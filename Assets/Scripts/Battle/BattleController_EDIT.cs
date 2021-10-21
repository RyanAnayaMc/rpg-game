using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleControllerEdit : MonoBehaviour {
    private enum WindowButton {
        None = -1,
        Attack = 0,
        Special = 1,
        Item = 2,
        Defend = 3
    }

    #region Input

    [HideInInspector]
    public static PlayerUnit inPlayerUnit;
    [HideInInspector]
    public static List<Unit> inEnemyUnits;
    [HideInInspector]
    public static AudioClip inMusic;
    [HideInInspector]
    public static string inScene;
    [HideInInspector]
    public static Transform inLocation;
    [HideInInspector]
    public static bool inParameters;

    #endregion

    #region Fields
    /// <summary>
    /// The prefab for the BattleUnit.
    /// </summary>
    [Header("Player and Enemy References")]
    /// <summary>
    /// The location to place the player
    /// </summary>
    public Transform playerLocation;

    /// <summary>
    /// The locations to place the enemy. Enemies that will
    /// spawn is limited to the number of enemy locations
    /// </summary>
    public List<Transform> enemyLocations;

    /// <summary>
    /// The Unit to spawn for the battle representing the enemy.
    /// A duplicate of the enemy Unit is spawned in.
    /// </summary>
    [SerializeField]
    private List<Unit> enemyUnitObjs;

    /// <summary>
    /// The BattleUnit instantiated from the player Unit
    /// </summary>
    [HideInInspector]
    public BattleUnit playerUnit;

    /// <summary>
    /// The BattleUnit instantiated from the enemy Unit
    /// </summary>
    [HideInInspector]
    public List<BattleUnit> enemyUnits;

    /// <summary>
    /// The current phase of the battle.
    /// </summary>
    BattlePhases phase;

    /// <summary>
    /// The BattleMusicHandler for the battle.
    /// </summary>
    [Header("Helper Scripts")]
    public BattleMusicHandler musicHandler;

    /// <summary>
    /// The BattleAnimationHandler for the battle.
    /// </summary>
    public BattleAnimationHandler animationHandler;

    /// <summary>
    /// The DamageCalculationHandler for the battle.
    /// </summary>
    public DamageCalculationHandler damageHandler;

    /// <summary>
    /// The BatlteUIHandler for the battle.
    /// </summary>
    public BattleUIHandler uiHandler;

    /// <summary>
    ///  The BattleSFXHandler for the battle.
    /// </summary>
    public BattleSFXHandler battleSFXHandler;

    /// <summary>
    /// The time in seconds to wait after something happens to
    /// let the player process what just happened
    /// </summary>
    public int waitTimer;

    /// <summary>
    /// SFX to play when a unit dies.
    /// </summary>
    [Header("Sound Effects")]
    public AudioClip enemyDefeatSFX;

    // Variables regarding the world screen
    private string currentScene;
    private bool isInAttackWindow;
    private Vector3 position;

    // Variables used by the battle system
    private bool canAct;
    private WindowButton caughtButton = WindowButton.None;
    private int selectedItem = -1;
    private int selectedEnemy = -1;
    private int selectedSpecial = -1;
    #endregion

    /// <summary>
    /// Starts a battle
    /// </summary>
    /// <param name="playerUnit">The player unit.</param>
    /// <param name="enemy">The enemy unit.</param>
    /// <param name="battleMusic">The battle music.</param>
    /// <param name="currentSceneName">The name of the scene you are currently in (to return to after the battle).</param>
    /// <param name="currentLocation">The location to return to after the battle.</param>
    /// <param name="battleSceneName">The name of the Battle scene to load</param>
    public static void StartBattle(List<Unit> enemies, AudioClip battleMusic, string scene, string battleSceneName = "CastleBattle") {
        CharacterMovementController.loadSavedLocation = true;
        CharacterMovementController.savedLocation = GameObject.FindGameObjectWithTag("Player").transform.position;

        SceneManager.LoadScene(battleSceneName);

        BattleController.inParameters = true;
        BattleController.inEnemyUnits = enemies;
        BattleController.inMusic = battleMusic;
        BattleController.inScene = scene;
    }

    void Start() {
        setupBattle();
    }

    private void setupBattle() {


        // Get parameters from other scene if present
        if (inParameters) {
            enemyUnitObjs = inEnemyUnits;
            musicHandler.battleMusic = inMusic;
            currentScene = inScene;
            inParameters = false;
        }

        // Play the battle music
        musicHandler.PlayBattleMusic();

        // Spawn units
        playerUnit = spawnPlayerUnit(PlayerUnit.INSTANCE, playerLocation);
        enemyUnits = spawnEnemies(enemyUnitObjs, enemyLocations);

        // Setup UI
        uiHandler.setupHUD(playerUnit, enemyUnits);
    }

    private async Task playerTurnStart() {
        // Show the player option window
        uiHandler.ShowPlayerOptionWindow();
        canAct = true;

        // Catch the pressed button and hide the menu
        WindowButton selectedButton = await catchPressedButton();
        battleSFXHandler.PlayConfirmSFX();
        uiHandler.HidePlayerOptionWindow();

        // Perform the next action based on what's happening
        switch (selectedButton) {
            case WindowButton.Attack:
                // Prompt user to select an enemy to attack
                 uiHandler.ShowEnemyTargetingWindow();
                //int attackTarget =  catchSelectedEnemy();
               // BattleUnit target = enemyUnits[attackTarget];
                //await attack(playerUnit, target);
                break;
            case WindowButton.Special:
                break;
            case WindowButton.Item:
                break;
            case WindowButton.Defend:
                break;
        }
    }

    private async Task attack(BattleUnit attacker, BattleUnit target) {
        // Determine what type of attack to do
        AttackType attackType = attacker.unit.weapon.atkType;

        switch (attackType) {
            case AttackType.Melee:
                await meleeAttack(attacker, target);
                break;
            case AttackType.Magic:
                await magicAttack(attacker, target);
                break;
            case AttackType.Ranged:
                await rangedAttack(attacker, target);
                break;
        }
    }

    private async Task meleeAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Calculate miss chance and determine if attack will miss
        float accuracy = 1 - ((float) (t.effAgi - t.effDex) / a.effDex);
        bool miss = UnityEngine.Random.value > accuracy;

        if (miss) {
            // Display output for a miss
            string message = a.unitName + " missed!";
            uiHandler.DisplayDialogueText(message);
            NumberPopup.DisplayTextPopup("Miss!", NumberType.NONE, target.transform);
        } else {
            // Calculate base damage
            int damage = a.effStr + a.weapon.might - t.effDef;

            // Do extra processing to the damage
            damage = offsetValue(damage, 0.1f);
            if (t.isDefending) damage /= 2;
            if (damage < 0) damage = 0;

            // Display animation
            target.DoAnimation(a.weapon.weaponAnimation);

            // Display damage output text and do damage
            NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, target.transform);
            target.TakeDamage(damage);
            string message = a.unitName + " did " + damage + " melee damage to " + t.unitName + ".";
            uiHandler.DisplayDialogueText(message);
        }
    }

    private async Task magicAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Calculate miss chance and determine if attack will miss
        float accuracy = 1 - ((float) (t.effAgi - t.effDex) / a.effDex);
        bool miss = UnityEngine.Random.value > accuracy;

        if (miss) {
            // Display output for a miss
            string message = a.unitName + " missed!";
            uiHandler.DisplayDialogueText(message);
            NumberPopup.DisplayTextPopup("Miss!", NumberType.NONE, target.transform);
        } else {
            // Calculate base damage
            int damage = a.effMag + a.weapon.might - t.effRes;

            // Do extra processing to the damage
            damage = offsetValue(damage, 0.1f);
            if (t.isDefending) damage /= 2;
            if (damage < 0) damage = 0;

            // Display animation
            target.DoAnimation(a.weapon.weaponAnimation);

            // Display damage output text and do damage
            NumberPopup.DisplayNumberPopup(damage, NumberType.Damage, target.transform);
            target.TakeDamage(damage);
            string message = a.unitName + " did " + damage + " magic damage to " + t.unitName + ".";
            uiHandler.DisplayDialogueText(message);
        }
    }

    private async Task rangedAttack(BattleUnit attacker, BattleUnit target) {
        // Get convenience references to the units
        Unit a = attacker.unit;
        Unit t = target.unit;

        // Calculate crit chance and determine if attack will crit
        float critChance = (float) (a.effDex - t.effAgi) / a.effDex;
        bool crit = UnityEngine.Random.value < critChance;

        // Calculate damage
        int damage = a.weapon.might;
        if (!crit) damage -= t.effArm;

        // Do extra processing to the damage
        damage = offsetValue(damage, 0.1f);
        if (t.isDefending) damage /= 2;
        if (damage < 0) damage = 0;

        // Display output text, animation, and do damage
        string message = a.unitName + " did " + damage + (crit ? " critical" : "") + " ranged damage to " + t.unitName + ".";
        target.DoAnimation(a.weapon.weaponAnimation);
        target.TakeDamage(damage);
        NumberPopup.DisplayTextPopup(damage + "!", NumberType.Damage, target.transform);
    }

    public void OnEnemyButton(int i) { selectedEnemy = i; }
    public void OnSkillButton(int i) { selectedSpecial = i; }
    public void OnItemButton(int i) { selectedItem = i; }

    #region Catchers
    private async Task<WindowButton> catchPressedButton() {
        while (caughtButton < 0) await Task.Delay(50);
        WindowButton button = caughtButton;
        caughtButton = WindowButton.None;
        return button;
    }

    private async Task<int> catchSelectedEnemy() {
        while (selectedEnemy < 0) await Task.Delay(50);
        int enemy = selectedEnemy;
        selectedEnemy = -1;
        return enemy;
    }

    private async Task<int> catchSelectedItem() {
        while (selectedItem < 0) await Task.Delay(50);
        int item = selectedItem;
        selectedItem = -1;
        return item;
	}

    private async Task<int> catchSelectedSpecial() {
        while (selectedSpecial < 0) await Task.Delay(50);
        int special = selectedSpecial;
        selectedSpecial = -1;
        return special;
	}
	#endregion

	#region Helper Methods
	private BattleUnit spawnPlayerUnit(PlayerUnit unit, Transform location) {
        BattleUnit battleUnit = Instantiate(unit.unitPrefab, location).GetComponent<BattleUnit>();
        battleUnit.unit = unit;
        battleUnit.unit.weapon = Instantiate(battleUnit.unit.weapon);
        return battleUnit;
	}

    private BattleUnit spawnEnemyUnit(Unit unit, Transform location) {
        BattleUnit battleUnit = Instantiate(unit.unitPrefab, location).GetComponent<BattleUnit>();
        battleUnit.unit = unit;
        battleUnit.unit.weapon = Instantiate(battleUnit.unit.weapon);
        return battleUnit;
    }

    private List<BattleUnit> spawnEnemies(List<Unit> units, List<Transform> locations) {
        List<BattleUnit> battleUnits = new List<BattleUnit>();

        for (int i = 0; i < units.Count && i < locations.Count; i++)
            battleUnits.Add(spawnEnemyUnit(units[i], locations[i]));

        return battleUnits;
	}

    public void OnBasicButton(int buttonPressed) {
        caughtButton = (WindowButton) buttonPressed;
    }

    private int offsetValue(int value, float offset) {
        // Randomly offsets an integer in the range of the offset
        float multiplier = UnityEngine.Random.Range(-offset, offset);

        return (int) (multiplier * value);
	}
    #endregion
}
