using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    #region Fields
    /// <summary>
    /// The prefab for the BattleUnit.
    /// </summary>
    [Header("Player and Enemy References")]
    public GameObject baseBattleUnit;

    /// <summary>
    /// The location to place the player
    /// </summary>
    public Transform playerLocation;

    /// <summary>
    /// The location to place the enemy
    /// </summary>
    public Transform enemyLocation;

    /// <summary>
    /// The Unit to spawn for the battle representing the player.
    /// </summary>
    [SerializeField]
    private Unit playerUnitObj;

    /// <summary>
    /// The Unit to spawn for the battle representing the enemy.
    /// A duplicate of the enemy Unit is spawned in.
    /// </summary>
    [SerializeField]
    private Unit enemyUnitObj;

    /// <summary>
    /// The BattleUnit instantiated from the player Unit
    /// </summary>
    [HideInInspector]
    public BattleUnit playerUnit;

    /// <summary>
    /// The BattleUnit instantiated from the enemy Unit
    /// </summary>
    [HideInInspector]
    public BattleUnit enemyUnit;

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
    /// The SkillMenuController for the battle.
    /// </summary>
    public SkillMenuController skillMenuController;

    /// <summary>
    ///  The BattleSFXHandler for the battle.
    /// </summary>
    public BattleSFXHandler battleSFXHandler;

    /// <summary>
    /// SFX to play when a unit dies.
    /// </summary>
    [Header("Sound Effects")]
    public AudioClip enemyDefeatSFX;
    #endregion

    #region Setup
    void Start() {
        StartCoroutine(BattleSetup());

    }
    private IEnumerator BattleSetup() {
        // Play the battle music
        musicHandler.PlayBattleMusic();

        // Spawn player and enemy
        phase = BattlePhase.START;
        GameObject playerObj = Instantiate(baseBattleUnit, playerLocation);
        GameObject enemyObj = Instantiate(baseBattleUnit, enemyLocation);

        // Setup sprites
        playerUnit = playerObj.GetComponent<BattleUnit>();
        playerUnit.GetComponent<SpriteRenderer>().flipX = true;
        enemyUnit = enemyObj.GetComponent<BattleUnit>();

        playerUnit.unit = playerUnitObj;
        enemyUnit.unit = Instantiate(enemyUnitObj);

        // Setup UI
        uiHandler.setupHUD(playerUnit.unit, enemyUnit.unit);

        yield return new WaitForSeconds(2);

        // Start player turn
        StartCoroutine(PlayerTurn());
    }
    #endregion

    #region Player Turn
    private IEnumerator PlayerTurn() {
        // Show player phase image
        uiHandler.ShowPlayerPhaseImage();

        yield return new WaitForSeconds(2);

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
        phase = BattlePhase.PLAYER_ACTION;
        battleSFXHandler.PlayConfirmSFX();

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        AttackType atkType = playerUnit.unit.weapon.atkType;
        StartCoroutine(PlayerAttack(atkType));
    }

    /// <summary>
    /// Code ran when the player clicks the item button.
    /// </summary>
    public void OnItemButton() {
        // TODO: show item menu
        if (phase != BattlePhase.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();
        StartCoroutine(onItemMenu());
        Debug.Log("Started coroutine");
    }

    private IEnumerator onItemMenu() {
        uiHandler.HidePlayerOptionWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowItemsWindow(PlayerInventory.INSTANCE.GetConsumableItems());
	}


    public void OnItemButton(int index) {
        phase = BattlePhase.PLAYER_ACTION;
        uiHandler.HideItemsWindow();

        Item item = PlayerInventory.INSTANCE.GetConsumableItems()[index].item;
        PlayerInventory.INSTANCE.RemoveItem(item);

        uiHandler.DisplayDialogueText(playerUnitObj.unitName + " uses a " + item.itemName + "!");
        StartCoroutine(useItem(item as Consumable));
    }

    private IEnumerator useItem(Consumable item) {
        Debug.Log(playerUnitObj.cHP + " " + enemyUnitObj.cHP);
        (string, bool) data = item.Use(playerUnit, enemyUnit, battleSFXHandler);
        Debug.Log(playerUnitObj.cHP + " " + enemyUnitObj.cHP);
        uiHandler.SetPlayerHUD(playerUnit.unit);
        uiHandler.SetEnemyHUD(enemyUnit.unit);
        yield return new WaitForSeconds(2);
        Debug.Log(playerUnitObj.cHP + " " + enemyUnitObj.cHP);
        uiHandler.DisplayDialogueText(data.Item1);
        yield return new WaitForSeconds(1);
        bool isDead = data.Item2;

        if (isDead)
            Win();
        else
            StartCoroutine(EnemyTurn());
    }

    /// <summary>
    /// Code ran when the player clicks the back button in the items menu
    /// </summary>
    public void OnItemBackButton() {
        StartCoroutine(ItemBackButton());
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
        }
        else {
            StartCoroutine(PlayerSkill(skill));
        }
    }

    private IEnumerator PlayerSkill(Skill skill) {
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

            if (isHeal)
            {
                playerUnit.Heal(data.Item2);
            }
            else
            {
                isDead = enemyUnit.TakeDamage(data.Item2);
            }
        }
        else {
            isDead = enemyUnit.unit.cHP <= 0;
        }

        uiHandler.SetPlayerHUD(playerUnit.unit);
        uiHandler.SetEnemyHUD(enemyUnit.unit);

        yield return new WaitForSeconds(2);
        if (isDead) {
            phase = BattlePhase.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

            StartCoroutine(Victory());
        }
        else {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }

    // Make the player defend themselves
    private IEnumerator PlayerDefend() {
        playerUnit.unit.isDefending = true;
        uiHandler.DisplayDialogueText(playerUnit.unit.unitName + " defends!");
        yield return new WaitForSeconds(2);
    }

    // Handles player attacking
    private IEnumerator PlayerAttack(AttackType type) {
        (string, int) attackData = damageHandler.NormalAttack(playerUnit.unit, enemyUnit.unit);

        // Sound effect and animation
        animationHandler.PlayDamageAnimation(playerUnit, enemyUnit, battleSFXHandler);

        // Check if enemy is dead
        bool isEnemyDead = enemyUnit.TakeDamage(attackData.Item2);
        uiHandler.SetEnemyHUD(enemyUnit.unit);
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);

        if (isEnemyDead) {
            phase = BattlePhase.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

            StartCoroutine(Victory());
        }
        else {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }
    #endregion

    #region Enemy Actions
    // Enemy Actions
    private IEnumerator EnemyTurn() {
        // Show enemy phase UI
        uiHandler.ShowEnemyPhaseImage();

        phase = BattlePhase.ENEMY;
        yield return new WaitForSeconds(2);

        // Do enemy attack
        AttackType atkType = enemyUnit.unit.weapon.atkType;
        StartCoroutine(EnemyAttack(atkType));

        // TODO more logic for enemy attack
    }

    private IEnumerator EnemyAttack(AttackType type) {
        (string, int) attackData = damageHandler.NormalAttack(enemyUnit.unit, playerUnit.unit);

        // Sound effect
        animationHandler.PlayDamageAnimation(enemyUnit, playerUnit, battleSFXHandler);

        // Check if player is dead
        bool isPlayerDead = playerUnit.TakeDamage(attackData.Item2);
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
        else {
            phase = BattlePhase.PLAYER;
            StartCoroutine(PlayerTurn());
        }
    }
    #endregion

    #region State Changes
    // Player wins
    public void Win() {
        phase = BattlePhase.WIN;

        // Fade out enemy
        battleSFXHandler.PlaySFX(enemyDefeatSFX);
        animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

        StartCoroutine(Victory());
    }

    private IEnumerator Victory() {
        uiHandler.DisplayDialogueText("You win!");
        musicHandler.PlayVictoryFanfare();
        
        yield return new WaitForSeconds(0);

        // TODO put player back to map screen
    }

    // Player lost
    private IEnumerator Lose() {
        // TODO make game over screen
        uiHandler.DisplayDialogueText("You lose...");
        yield return new WaitForSeconds(0);
    }
    #endregion
}
