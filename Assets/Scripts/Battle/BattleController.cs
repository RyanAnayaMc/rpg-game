using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// A class that handles the bulk of combat logic

public enum BattlePhase
{
    START, // Battle is being setup
    PLAYER, // Waiting for player to pick an option
    PLAYER_ACTION, // Player is perfomring an action
    ENEMY, // Enemy is performing an action
    WIN, // Enemy was defeated
    LOSE // Player was defeated
}

public class BattleController : MonoBehaviour
{
    #region Fields
    // Player and Enemy References
    public GameObject baseBattleUnit; // The prefab for a BattleUnit

    public Transform playerLocation; // The location to place the player
    public Transform enemyLocation; // The location to place the enemy

    [SerializeField]
    private Unit playerUnitObj;
    [SerializeField]
    private Unit enemyUnitObj;

    [HideInInspector]
    public BattleUnit playerUnit; // The Unit linked to the player GameObject
    [HideInInspector]
    public BattleUnit enemyUnit; // The Unit linked to the enemy GameObject

    // Battle phase
    BattlePhase phase; // Current phase of the battle

    // Helper scripts
    public BattleMusicHandler musicHandler; // The BattleMusicHandler for the battle
    public BattleAnimationHandler animationHandler; // The BattleAnimationHandler for the battle
    public DamageCalculationHandler damageHandler; // The DamageCalculationHandler for the battle
    public BattleUIHandler uiHandler; // The BatlteUIHandler for the battle
    public SkillMenuController skillMenuController; // The SkillMenuController for the battle
    public BattleSFXHandler battleSFXHandler; // The BattleSFXHandler for the battle

    // Battle sound effects
    public AudioClip enemyDefeatSFX; // SFX that plays when an enemy dies
    #endregion

    #region Setup
    void Start()
    {
        StartCoroutine(BattleSetup());
    }

    // Coroutine to handle setting up the battle
    IEnumerator BattleSetup()
    {
        // Play the battle music
        musicHandler.PlayBattleMusic();

        // Spawn player and enemy
        phase = BattlePhase.START;
        GameObject playerObj = Instantiate(baseBattleUnit, playerLocation);
        GameObject enemyObj = Instantiate(baseBattleUnit, enemyLocation);

        // Setup sprites
        playerUnit = playerObj.GetComponent<BattleUnit>();
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
    // Player Actions
    IEnumerator PlayerTurn()
    {
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

    // Player clicks attack button
    public void OnAttackButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;
        phase = BattlePhase.PLAYER_ACTION;
        battleSFXHandler.PlayConfirmSFX();

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        AttackType atkType = playerUnit.unit.weapon.atkType;
        StartCoroutine(PlayerAttack(atkType));
    }

    // Player clicks item button
    public void OnItemButton()
    {
        // TODO: show item menu
        if (phase != BattlePhase.PLAYER)
            return;

        // battleSFXHandler.PlayConfirmSFX();
        // phase = BattlePhase.PLAYER_ACTION;
        // Hide option menu
        // StartCoroutine(stretchOut(playerOptionWindow, 0.2f));
    }

    // Player clicks defend button
    public void OnDefendButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        phase = BattlePhase.PLAYER_ACTION;
        StartCoroutine(PlayerDefend());
        StartCoroutine(EnemyTurn());
    }

    // Player clicks the skills menu button
    public void onSkillsMenuButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        battleSFXHandler.PlayConfirmSFX();
        StartCoroutine(onSkillMenu());
    }

    private IEnumerator onSkillMenu()
    {
        uiHandler.HidePlayerOptionWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowSkillWindow();
    }

    // Player clicksk the back button on the skill menu
    public void onSkillsBackButton()
    {
        battleSFXHandler.PlayBackSFX();
        StartCoroutine(skillBackButton());
    }

    private IEnumerator skillBackButton()
    {
        uiHandler.HideSkillWindow();
        yield return new WaitForSeconds(0.3f);
        uiHandler.ShowPlayerOptionWindow();
    }

    // Player clicks a skill button
    public void onSkillButton(int index)
    {
        
        Debug.Log("Skill " + index);

        Skill skill = playerUnit.unit.skills[index];
        if (playerUnit.unit.cSP < skill.costSP)
        {
            battleSFXHandler.PlayBackSFX();
            uiHandler.DisplayDialogueText("You do not have enough SP to use " + skill.skillName + "!");
        }
        else
        {
            StartCoroutine(PlayerSkill(skill));
        }
    }

    IEnumerator PlayerSkill(Skill skill)
    {
        bool isDead = false;
        uiHandler.HideSkillWindow();
        battleSFXHandler.PlayConfirmSFX();
        playerUnit.unit.cSP -= skill.costSP;
        (string, int, bool) data = damageHandler.SpecialAttack(skill, playerUnit, enemyUnit);

        yield return new WaitForSeconds(0.2f);

        uiHandler.DisplayDialogueText(data.Item1);

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
        Debug.Log(data.Item1);
        uiHandler.SetPlayerHUD(playerUnit.unit);
        uiHandler.SetEnemyHUD(enemyUnit.unit);

        yield return new WaitForSeconds(2);
        Debug.Log("waited");
        if (isDead)
        {
            phase = BattlePhase.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

            StartCoroutine(Victory());
        }
        else
        {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }

    // Make the player defend themselves
    IEnumerator PlayerDefend()
    {
        playerUnit.unit.isDefending = true;
        uiHandler.DisplayDialogueText(playerUnit.unit.unitName + " defends!");
        yield return new WaitForSeconds(2);
    }

    // Handles player attacking
    IEnumerator PlayerAttack(AttackType type)
    {
        (string, int) attackData = damageHandler.NormalAttack(playerUnit.unit, enemyUnit.unit);

        // Sound effect and animation
        animationHandler.PlayDamageAnimation(playerUnit, enemyUnit, battleSFXHandler);

        // Check if enemy is dead
        bool isEnemyDead = enemyUnit.TakeDamage(attackData.Item2);
        uiHandler.SetEnemyHUD(enemyUnit.unit);
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);

        if (isEnemyDead)
        {
            phase = BattlePhase.WIN;

            // Fade out enemy
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

            StartCoroutine(Victory());
        }
        else
        {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }
    #endregion

    #region Enemy Actions
    // Enemy Actions
    IEnumerator EnemyTurn()
    {
        // Show enemy phase UI
        uiHandler.ShowEnemyPhaseImage();

        phase = BattlePhase.ENEMY;
        yield return new WaitForSeconds(2);

        // Do enemy attack
        AttackType atkType = enemyUnit.unit.weapon.atkType;
        StartCoroutine(EnemyAttack(atkType));

        // TODO more logic for enemy attack
    }

    IEnumerator EnemyAttack(AttackType type)
    {
        (string, int) attackData = damageHandler.NormalAttack(enemyUnit.unit, playerUnit.unit);

        // Sound effect
        animationHandler.PlayDamageAnimation(enemyUnit, playerUnit, battleSFXHandler);

        // Check if player is dead
        bool isPlayerDead = playerUnit.TakeDamage(attackData.Item2);
        uiHandler.SetPlayerHUD(playerUnit.unit);
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);

        if (isPlayerDead)
        {
            phase = BattlePhase.LOSE;

            // Fade out player
            battleSFXHandler.PlaySFX(enemyDefeatSFX);
            animationHandler.fadeOutSprite(playerUnit.gameObject, 0.1f);

            StartCoroutine(Lose());
        }
        else
        {
            phase = BattlePhase.PLAYER;
            StartCoroutine(PlayerTurn());
        }
    }
    #endregion

    #region State Changes
    // Player wins
    IEnumerator Victory()
    {
        uiHandler.DisplayDialogueText("You win!");
        musicHandler.PlayVictoryFanfare();
        
        yield return new WaitForSeconds(0);

        // TODO put player back to map screen
    }

    // Plays the victory fanfare sound, then victory music after


    // Player lost
    IEnumerator Lose()
    {
        // TODO make game over screen
        uiHandler.DisplayDialogueText("You lose...");
        yield return new WaitForSeconds(0);
    }
    #endregion
}
