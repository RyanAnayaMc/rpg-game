using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// A class that handles the bulk of combat logic

public enum BattlePhases {
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
    /// SFX to play when a unit dies.
    /// </summary>
    [Header("Sound Effects")]
    public AudioClip enemyDefeatSFX;

    private string currentScene;
    private bool isInAttackWindow;
    private Skill chosenSkill;
    private Vector3 position;
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
            enemyUnitObjs = inEnemyUnits;
            musicHandler.battleMusic = inMusic;
            currentScene = inScene;
            inParameters = false;
        }

        // Play the battle music
        musicHandler.PlayBattleMusic();

        // Spawn player
        phase = BattlePhases.START;
        playerUnit = Instantiate(PlayerUnit.INSTANCE.unitPrefab, playerLocation)
            .GetComponent<BattleUnit>();
        playerUnit.unit = PlayerUnit.INSTANCE;
        playerUnit.unit.weapon = Instantiate(playerUnit.unit.weapon);

        // Spawn enemies
        List<BattleUnit> enemies = new List<BattleUnit>();
        for (int i = 0; i < enemyLocations.Count && i < enemyUnitObjs.Count; i++) {
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
        phase = BattlePhases.PLAYER;
        playerUnit.unit.isDefending = false;
        uiHandler.DisplayDialogueText("Choose an action.");

    }

    /// <summary>
    /// Code ran when the player clicks the attack button.
    /// </summary>
    public void OnAttackButton() {
        if (phase != BattlePhases.PLAYER)
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
            phase = BattlePhases.PLAYER_ACTION;
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
        if (phase != BattlePhases.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();
        StartCoroutine(onItemMenu());
    }

    private IEnumerator onItemMenu() {
        uiHandler.HidePlayerOptionWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowItemsWindow(Inventory.INSTANCE.GetConsumables());
    }


    public void OnItemButton(int index) {
        phase = BattlePhases.PLAYER_ACTION;
        uiHandler.HideItemsWindow();

        battleSFXHandler.PlayConfirmSFX();

        Consumable item = Inventory.INSTANCE.GetConsumables()[index].item as Consumable;
        Inventory.INSTANCE.RemoveConsumable(Atlas.GetID(item));

        uiHandler.DisplayDialogueText(playerUnit.unit.unitName + " uses a " + item.itemName + "!");
        StartCoroutine(useItem(item as Consumable));
    }


    private IEnumerator useItem(Consumable item) {
        string dialogueText = item.Use(playerUnit, battleSFXHandler);

        if (item.type == ConsumableType.DamageDeal) {
            yield return new WaitForSeconds(2);

            foreach (BattleUnit enemy in enemyUnits) {
                enemy.UpdateHUD();
                for (int i = 0; i < enemyUnits.Count; i++) {
                    if (enemyUnits[i].unit.cHP <= 0) {
                        animationHandler.fadeOutSprite(enemyUnits[i].gameObject, 0.1f);
                        enemyUnits.Remove(enemyUnits[i]);
                        i--;
                    }
                }
            }

            if (enemyUnits.Count <= 0)
                Win();
        } else
            playerUnit.UpdateHUD();

        yield return new WaitForSeconds(2);

        StartCoroutine(EnemyTurn());
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
        if (phase != BattlePhases.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        phase = BattlePhases.PLAYER_ACTION;
        StartCoroutine(PlayerDefend());
        StartCoroutine(EnemyTurn());
    }

    /// <summary>
    /// Code ran when the player clicks the skills menu button.
    /// </summary>
    public void onSkillsMenuButton() {
        if (phase != BattlePhases.PLAYER)
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
    public void OnSkillButton(int index) {
        Skill skill = playerUnit.unit.skills[index];
        if (playerUnit.unit.cSP < skill.costSP) {
            battleSFXHandler.PlayBackSFX();
            uiHandler.DisplayDialogueText("You do not have enough SP to use " + skill.skillName + "!");
        } else {
            battleSFXHandler.PlayConfirmSFX();
            playerUnit.unit.cSP -= skill.costSP;

            if (skill.targetType == TargetType.ENEMY || (skill.useScriptedSkillEffects && skill.needsTarget)) {
                chosenSkill = skill;
                uiHandler.HideSkillWindow();
                uiHandler.ShowEnemyTargetingWindow();
            } else {
                uiHandler.HideSkillWindow();
                StartCoroutine(PlayerSkill(skill, null));
            }
        }
    }

    private IEnumerator PlayerSkill(Skill skill, BattleUnit enemyUnit) {
        string diagMessage = skill.DoSkill(playerUnit, enemyUnit, battleSFXHandler);
        uiHandler.DisplayDialogueText(diagMessage);

        playerUnit.UpdateHUD();

        if (enemyUnit != null)
            enemyUnit.UpdateHUD();
        else foreach (BattleUnit enemy in enemyUnits)
                enemy.UpdateHUD();

        yield return new WaitForSeconds(2);

        // Remove enemies if killed
        if (enemyUnit != null) {
            if (enemyUnit.unit.cHP <= 0) {
                enemyUnits.Remove(enemyUnit);
                animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);
            }
        } else {
            for (int i = 0; i < enemyUnits.Count; i++) {
                if (enemyUnits[i].unit.cHP <= 0) {
                    animationHandler.fadeOutSprite(enemyUnits[i].gameObject, 0.1f);
                    enemyUnits.Remove(enemyUnits[i]);
                    i--;
                }
            }
        }

        if (enemyUnits.Count <= 0)
            Win();
        else
            StartCoroutine(EnemyTurn());
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
            phase = BattlePhases.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(target.gameObject, 0.1f);

            enemyUnits.Remove(target);

            if (enemyUnits.Count == 0)
                Win();
            else {
                phase = BattlePhases.ENEMY;
                StartCoroutine(EnemyTurn());
            }
        } else {
            phase = BattlePhases.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }

    #endregion

    #region Enemy Actions
    // Enemy Actions
    private IEnumerator EnemyTurn() {
        // Show enemy phase UI
        // uiHandler.ShowEnemyPhaseImage();
        phase = BattlePhases.ENEMY;

        foreach (BattleUnit enemy in enemyUnits) {
            EnemyAction(enemy);
            yield return new WaitForSeconds(2);
        }

        if (playerUnit.unit.cHP > 0)
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
            phase = BattlePhases.LOSE;

            // Fade out player
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(playerUnit.gameObject, 0.1f);

            StartCoroutine(Lose());
        }
    }

    private IEnumerator EnemySkill(Skill skill, BattleUnit enemyUnit) {
        enemyUnit.unit.cSP -= skill.costSP;
        string diagMessage = skill.DoSkill(enemyUnit, playerUnit, battleSFXHandler);

        uiHandler.DisplayDialogueText(diagMessage);

        playerUnit.UpdateHUD();
        enemyUnit.UpdateHUD();

        yield return new WaitForSeconds(2);
        enemyUnit.unitHUD.HideBars();

        if (playerUnit.unit.cHP <= 0) {
            phase = BattlePhases.LOSE;

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
        phase = BattlePhases.WIN;

        StartCoroutine(Victory());
    }

    private IEnumerator Victory() {
        uiHandler.DisplayDialogueText("You win!");
        musicHandler.PlayVictoryFanfare();

        yield return new WaitUntil(() => Input.GetButtonDown("Interact"));

        SceneManager.LoadScene(currentScene);
    }

    // Player lost
    private IEnumerator Lose() {
        // TODO: make game over screen
        uiHandler.DisplayDialogueText("You lose...");
        yield return new WaitForSeconds(0);
    }
    #endregion
}
