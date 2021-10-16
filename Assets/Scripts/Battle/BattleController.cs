using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// A class that handles the bulk of combat logic

public enum BattlePhase {
    START, // Battle is being setup
    PLAYER, // Waiting for player to pick an option
    PLAYER_ACTION, // Player is perfomring an action
    ENEMY, // Enemy is performing an action
    WIN, // Enemy was defeated
    LOSE // Player was defeated
}

public class BattleController : MonoBehaviour {
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
    /// The Unit to spawn for the battle representing the player.
    /// </summary>
    [SerializeField]
    private PlayerUnit playerUnitObj;

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
    BattlePhase phase;

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
    /// SFX to play when a unit dies.
    /// </summary>
    [Header("Sound Effects")]
    public AudioClip enemyDefeatSFX;

    private string currentScene;
    private Transform location;
    private bool isInAttackWindow;
    private Skill chosenSkill;
    #endregion

    #region Setup
    /// <summary>
    /// Starts a battle
    /// </summary>
    /// <param name="playerUnit">The player unit.</param>
    /// <param name="enemy">The enemy unit.</param>
    /// <param name="battleMusic">The battle music.</param>
    /// <param name="currentSceneName">The name of the scene you are currently in (to return to after the battle).</param>
    /// <param name="currentLocation">The location to return to after the battle.</param>
    /// <param name="battleSceneName">The name of the Battle scene to load</param>
    public static void StartBattle(PlayerUnit playerUnit, List<Unit> enemies, AudioClip battleMusic, string currentSceneName, Transform currentLocation, string battleSceneName = "CastleBattle") {
        SceneManager.LoadScene(battleSceneName);

        BattleController.inParameters = true;
        BattleController.inPlayerUnit = playerUnit;
        BattleController.inEnemyUnits = enemies;
        BattleController.inMusic = battleMusic;
        BattleController.inScene = currentSceneName;
        BattleController.inLocation = currentLocation;
    }

    void Start() {
        StartCoroutine(BattleSetup());

    }
    private IEnumerator BattleSetup() {
        // Setup helpers
        battleSFXHandler.battleController = this;
        uiHandler.battleController = this;
        musicHandler.battleController = this;
        animationHandler.battleController = this;
        damageHandler.battleController = this;
        

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Get parameters from other scene if present
        if (inParameters) {
            playerUnitObj = inPlayerUnit;
            enemyUnitObjs = inEnemyUnits;
            musicHandler.battleMusic = inMusic;
            currentScene = inScene;
            location = inLocation;
            inParameters = false;
        }

        // Play the battle music
        musicHandler.PlayBattleMusic();

        // Spawn player
        phase = BattlePhase.START;
        playerUnitObj = Instantiate(playerUnitObj);
        playerUnit = Instantiate(playerUnitObj.unitPrefab, playerLocation)
            .GetComponent<BattleUnit>();
        playerUnit.unit = playerUnitObj;
        playerUnit.unit.weapon = Instantiate(playerUnit.unit.weapon);

        // Spawn enemies
        List<BattleUnit> enemies = new List<BattleUnit>();
        for (int i = 0; i < enemyLocations.Count && i < enemyUnitObjs.Count; i++) {
            Debug.Log(i);
            enemyUnitObjs[i] = Instantiate(enemyUnitObjs[i]);
            BattleUnit enemy = Instantiate(enemyUnitObjs[i].unitPrefab, enemyLocations[i])
                .GetComponent<BattleUnit>();
            enemy.unit = enemyUnitObjs[i];
            enemy.unit.weapon = Instantiate(enemy.unit.weapon);
            enemies.Add(enemy);
		}

        enemyUnits = enemies;

        // Setup UI
        uiHandler.setupHUD(playerUnit, enemyUnits);

        yield return new WaitForSeconds(2);

        // Start player turn
        StartCoroutine(PlayerTurn());
    }
    #endregion

    #region Player Turn
    private IEnumerator PlayerTurn() {
        // Show player phase image
        // uiHandler.ShowPlayerPhaseImage();

        yield return new WaitForSeconds(1);

        // Show option menu
        uiHandler.ShowPlayerOptionWindow();

        // Let player pick an option
        phase = BattlePhase.PLAYER;
        playerUnit.unit.isDefending = false;
        uiHandler.DisplayDialogueText("Choose an action.");

    }

    /// <summary>
    /// Code ran when the player clicks the attack button.
    /// </summary>
    public void OnAttackButton() {
        if (phase != BattlePhase.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();
        isInAttackWindow = true;

        // Prompt user to target enemy unit
        uiHandler.HidePlayerOptionWindow();
        uiHandler.ShowEnemyTargetingWindow();
    }

    public void OnEnemyButton(int index) {
        BattleUnit enemy = enemyUnits[index];

        if (isInAttackWindow) {
            // Do regular attack on enemy
            phase = BattlePhase.PLAYER_ACTION;
            battleSFXHandler.PlayConfirmSFX();

            // Hide option menu
            uiHandler.HideEnemyTargetingWindow();

            AttackType atkType = playerUnit.unit.weapon.atkType;
            StartCoroutine(PlayerAttack(atkType, enemy));
        } else {
            // Do special attack on enemy
            battleSFXHandler.PlayConfirmSFX();

            // Hide option menu
            uiHandler.HideEnemyTargetingWindow();

            // Do special
            StartCoroutine(PlayerSkill(chosenSkill, enemy));
        }
    }

    public void OnEnemyBackButton() {
        StartCoroutine(EnemyBackButton());
        battleSFXHandler.PlayBackSFX();
    }

    private IEnumerator EnemyBackButton() {
        uiHandler.HideEnemyTargetingWindow();
        yield return new WaitForSeconds(0.3f);
        if (isInAttackWindow)
            uiHandler.ShowPlayerOptionWindow();
        else
            uiHandler.ShowSkillWindow();
    }

    /// <summary>
    /// Code ran when the player clicks the item button.
    /// </summary>
    public void OnItemButton() {
        if (phase != BattlePhase.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();
        StartCoroutine(onItemMenu());
    }

    private IEnumerator onItemMenu() {
        uiHandler.HidePlayerOptionWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowItemsWindow(PlayerInventory.INSTANCE.GetConsumableItems());
    }


    public void OnItemButton(int index) {
        phase = BattlePhase.PLAYER_ACTION;
        uiHandler.HideItemsWindow();

        battleSFXHandler.PlayConfirmSFX();

        Item item = PlayerInventory.INSTANCE.GetConsumableItems()[index].item;
        PlayerInventory.INSTANCE.RemoveItem(item);

        uiHandler.DisplayDialogueText(playerUnitObj.unitName + " uses a " + item.itemName + "!");
        StartCoroutine(useItem(item as Consumable));
    }

    private IEnumerator useItem(Consumable item) {
        /*
        (string, bool) data = item.Use(playerUnit, enemyUnit, battleSFXHandler);
        playerUnit.UpdateHUD();
        enemyUnit.UpdateHUD();
        yield return new WaitForSeconds(2);
        enemyUnit.unitHUD.HideBars();
        uiHandler.DisplayDialogueText(data.Item1);
        yield return new WaitForSeconds(1);
        bool isDead = data.Item2;


        if (isDead)
            Win();
        else
            StartCoroutine(EnemyTurn());
        */
        yield return null;
    }

    /// <summary>
    /// Code ran when the player clicks the back button in the items menu
    /// </summary>
    public void OnItemBackButton() {
        StartCoroutine(ItemBackButton());
        battleSFXHandler.PlayBackSFX();
    }

    private IEnumerator ItemBackButton() {
        uiHandler.HideItemsWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowPlayerOptionWindow();
    }

    /// <summary>
    /// Code ran when the player clicks the defend button.
    /// </summary>
    public void OnDefendButton() {
        if (phase != BattlePhase.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        phase = BattlePhase.PLAYER_ACTION;
        StartCoroutine(PlayerDefend());
        StartCoroutine(EnemyTurn());
    }

    /// <summary>
    /// Code ran when the player clicks the skills menu button.
    /// </summary>
    public void onSkillsMenuButton() {
        if (phase != BattlePhase.PLAYER)
            return;

        isInAttackWindow = false;
        battleSFXHandler.PlayConfirmSFX();
        StartCoroutine(onSkillMenu());
    }

    private IEnumerator onSkillMenu() {
        uiHandler.HidePlayerOptionWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowSkillWindow();
    }

    /// <summary>
    /// Code ran when the player leaves the skills menu.
    /// </summary>
    public void onSkillsBackButton() {
        battleSFXHandler.PlayBackSFX();
        StartCoroutine(skillBackButton());
    }

    private IEnumerator skillBackButton() {
        uiHandler.HideSkillWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowPlayerOptionWindow();
    }

    /// <summary>
    /// Code ran when the player clicks a skill button.
    /// </summary>
    /// <param name="index">The skill button pressed.</param>
    public void onSkillButton(int index) {
        Skill skill = playerUnit.unit.skills[index];
        if (playerUnit.unit.cSP < skill.costSP) {
            battleSFXHandler.PlayBackSFX();
            uiHandler.DisplayDialogueText("You do not have enough SP to use " + skill.skillName + "!");
        } else {
            battleSFXHandler.PlayConfirmSFX();
            chosenSkill = skill;
            uiHandler.HideSkillWindow();

            uiHandler.ShowEnemyTargetingWindow();
        }
    }

    private IEnumerator PlayerSkill(Skill skill, BattleUnit enemyUnit) {
        bool isDead = false;
        uiHandler.HideSkillWindow();
        battleSFXHandler.PlayConfirmSFX();
        playerUnit.unit.cSP -= skill.costSP;
        (string, int, bool, bool) data = damageHandler.SpecialAttack(skill, playerUnit, enemyUnit, battleSFXHandler);

        yield return new WaitForSeconds(0.2f);

        bool wasScripted = data.Item4;
        uiHandler.DisplayDialogueText(data.Item1);

        if (!wasScripted) {
            bool isHeal = !data.Item3;

            animationHandler.PlaySkillAnimation(playerUnit, enemyUnit, skill, battleSFXHandler);

            if (isHeal) {
                int heal = playerUnit.Heal(data.Item2);
                NumberPopup.DisplayNumberPopup(heal, NumberType.Heal, playerUnit.transform);
            } else {
                enemyUnit.unitHUD.ShowHPBar();
                isDead = enemyUnit.TakeDamage(data.Item2);
                NumberPopup.DisplayNumberPopup(data.Item2, NumberType.Damage, enemyUnit.transform);
            }
        } else {
            isDead = enemyUnit.unit.cHP <= 0;
        }

        playerUnit.UpdateHUD();
        enemyUnit.UpdateHUD();

        yield return new WaitForSeconds(2);
        enemyUnit.unitHUD.HideBars();
        if (isDead) {
            phase = BattlePhase.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

            StartCoroutine(Victory());
        } else {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
        yield return null;
    }

    // Make the player defend themselves
    private IEnumerator PlayerDefend() {
        playerUnit.unit.isDefending = true;
        uiHandler.DisplayDialogueText(playerUnit.unit.unitName + " defends!");
        yield return new WaitForSeconds(2);
    }

    // Handles player attacking
    private IEnumerator PlayerAttack(AttackType type, BattleUnit target) {
        (string, int) attackData = damageHandler.NormalAttack(playerUnit.unit, target.unit);

        // Sound effect and animation
        target.unitHUD.ShowHPBar();
        animationHandler.PlayDamageAnimation(playerUnit, target, battleSFXHandler);

        // Check if enemy is dead
        bool isEnemyDead = target.TakeDamage(attackData.Item2);
        NumberPopup.DisplayNumberPopup(attackData.Item2, NumberType.Damage, target.transform);
        target.UpdateHP();
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);
        target.unitHUD.HideHPBar();

        if (isEnemyDead) {
            phase = BattlePhase.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(target.gameObject, 0.1f);

            Win();
        } else {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }

    #endregion

    #region Enemy Actions
    // Enemy Actions
    private IEnumerator EnemyTurn() {
        // Show enemy phase UI
        // uiHandler.ShowEnemyPhaseImage();
        phase = BattlePhase.ENEMY;

        foreach (BattleUnit enemy in enemyUnits) {
            EnemyAction(enemy);
            yield return new WaitForSeconds(4);
        }

        StartCoroutine(PlayerTurn());
    }

    private void EnemyAction(BattleUnit enemyUnit) {
        // Determine if enemy should attack or use a special

        // Get random skill if enemy has skills
        Skill skill = null;
        if (enemyUnit.unit.skills.Count > 1) {
            int index = Random.Range(0, enemyUnit.unit.skills.Count - 1);
            skill = enemyUnit.unit.skills[index];
            if (enemyUnit.unit.cSP < skill.costSP)
                skill = null;
        }

        // Determine if enemy should do skill
        int atkWeight = Mathf.Clamp(enemyUnit.unit.skills.Count, 1, 3);
        float atkChance = (float) atkWeight / (float) (atkWeight + enemyUnit.unit.skills.Count);
        bool doRegularAttack = Random.value < atkChance || skill is null;

        // Do enemy attack
        if (doRegularAttack) {
            AttackType atkType = enemyUnit.unit.weapon.atkType;
            StartCoroutine(EnemyAttack(atkType, enemyUnit));
        } else
            StartCoroutine(EnemySkill(skill, enemyUnit));
		// TODO: more logic for enemy attack
	}

	private IEnumerator EnemyAttack(AttackType type, BattleUnit enemyUnit) {
        (string, int) attackData = damageHandler.NormalAttack(enemyUnit.unit, playerUnit.unit);

        // Sound effect
        animationHandler.PlayDamageAnimation(enemyUnit, playerUnit, battleSFXHandler);

        // Check if player is dead
        bool isPlayerDead = playerUnit.TakeDamage(attackData.Item2);
        NumberPopup.DisplayNumberPopup(attackData.Item2, NumberType.Damage, playerUnit.transform);
        uiHandler.SetPlayerHUD(playerUnit.unit);
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);

        if (isPlayerDead) {
            phase = BattlePhase.LOSE;

            // Fade out player
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(playerUnit.gameObject, 0.1f);

            StartCoroutine(Lose());
        }
    }

    private IEnumerator EnemySkill(Skill skill, BattleUnit enemyUnit) {
        bool isDead = false;
        enemyUnit.unit.cSP -= skill.costSP;
        (string, int, bool, bool) data = damageHandler.SpecialAttack(skill, enemyUnit, playerUnit, battleSFXHandler);

        yield return new WaitForSeconds(0.2f);

        bool wasScripted = data.Item4;
        uiHandler.DisplayDialogueText(data.Item1);

        if (!wasScripted) {
            bool isHeal = !data.Item3;

            animationHandler.PlaySkillAnimation(enemyUnit, playerUnit, skill, battleSFXHandler);

            if (isHeal) {
                int heal = enemyUnit.Heal(data.Item2);
                NumberPopup.DisplayNumberPopup(heal, NumberType.Heal, enemyUnit.transform);
            } else {
                isDead = playerUnit.TakeDamage(data.Item2);
                NumberPopup.DisplayNumberPopup(data.Item2, NumberType.Damage, playerUnit.transform);
            }
        } else
            isDead = enemyUnit.unit.cHP <= 0;

        playerUnit.UpdateHUD();
        enemyUnit.UpdateHUD();

        yield return new WaitForSeconds(2);
        enemyUnit.unitHUD.HideBars();

        if (isDead) {
            phase = BattlePhase.LOSE;

            // Fade out player
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(playerUnit.gameObject, 0.1f);

            StartCoroutine(Lose());
		}
	}
    #endregion

    #region State Changes
    // Player wins
    public void Win() {
        phase = BattlePhase.WIN;

        StartCoroutine(Victory());
    }

    private IEnumerator Victory() {
        uiHandler.DisplayDialogueText("You win!");
        musicHandler.PlayVictoryFanfare();
        
        yield return new WaitForSeconds(0);

        // TODO: put player back to map screen
    }

    // Player lost
    private IEnumerator Lose() {
        // TODO: make game over screen
        uiHandler.DisplayDialogueText("You lose...");
        yield return new WaitForSeconds(0);
    }
    #endregion
}
