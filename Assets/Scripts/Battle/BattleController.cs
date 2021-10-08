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
    public GameObject player; // Prefab to represent the player
    public GameObject enemy; // Prefab to represent the enemy

    BattlePhase phase; // Current phase of the battle

    public BattleMusicHandler musicHandler; // The BattleMusicHandler for the battle
    public BattleAnimationHandler animationHandler; // The BattleAnimationHandler for the battle
    public DamageCalculationHandler damageHandler; // The DamageCalculationHandler for the battle
    public BattleUIHandler uiHandler; // The BatlteUIHandler for the battle

    public AudioClip enemyDefeatSFX; // SFX that plays when an enemy dies
    public AudioSource sfxSource; // The scene's sound effect player

    public Transform playerLocation; // The location to place the player
    public Transform enemyLocation; // The location to place the enemy

    Unit playerUnit; // The Unit linked to the player GameObject
    Unit enemyUnit; // The Unit linked to the enemy GameObject



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
        GameObject playerObj = Instantiate(player, playerLocation);
        GameObject enemyObj = Instantiate(enemy, enemyLocation);

        playerUnit = playerObj.GetComponent<Unit>();
        enemyUnit = enemyObj.GetComponent<Unit>();

        // Setup UI
        uiHandler.setupHUD(playerUnit, enemyUnit);

        yield return new WaitForSeconds(2);

        // Start player turn
        StartCoroutine(PlayerTurn());
    }

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
        playerUnit.isDefending = false;
        uiHandler.DisplayDialogueText("Choose an action.");
        
    }

    // Player clicks attack button
    public void OnAttackButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;
        phase = BattlePhase.PLAYER_ACTION;

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        AttackType atkType = playerUnit.weapon.atkType;
        StartCoroutine(PlayerAttack(atkType));
    }

    // Player clicks item button
    public void OnItemButton()
    {
        // TODO: show item menu
        if (phase != BattlePhase.PLAYER)
            return;
        // phase = BattlePhase.PLAYER_ACTION;
        // Hide option menu
        // StartCoroutine(stretchOut(playerOptionWindow, 0.2f));
    }

    // Player clicks defend button
    public void OnDefendButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        // Hide option menu
        uiHandler.HidePlayerOptionWindow();

        phase = BattlePhase.PLAYER_ACTION;
        StartCoroutine(PlayerDefend());
        StartCoroutine(EnemyTurn());
    }

    // Make the player defend themselves
    IEnumerator PlayerDefend()
    {
        playerUnit.isDefending = true;
        uiHandler.DisplayDialogueText(playerUnit.unitName + " defends!");
        yield return new WaitForSeconds(2);
    }

    // Handles player attacking
    IEnumerator PlayerAttack(AttackType type)
    {
        (string, int) attackData = damageHandler.NormalAttack(playerUnit, enemyUnit);

        // Sound effect and animation
        animationHandler.PlayDamageAnimation(playerUnit, enemyUnit, sfxSource);

        // Check if enemy is dead
        bool isEnemyDead = enemyUnit.TakeDamage(attackData.Item2);
        uiHandler.SetEnemyHUD(enemyUnit);
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);

        if (isEnemyDead)
        {
            phase = BattlePhase.WIN;

            // Fade out enemy
            sfxSource.PlayOneShot(enemyDefeatSFX);
            animationHandler.fadeOutSprite(enemyUnit.gameObject, 0.1f);

            StartCoroutine(Victory());
        }
        else
        {
            phase = BattlePhase.ENEMY;
            StartCoroutine(EnemyTurn());
        }
    }

    // Enemy Actions
    IEnumerator EnemyTurn()
    {
        // Show enemy phase UI
        uiHandler.ShowEnemyPhaseImage();

        phase = BattlePhase.ENEMY;
        yield return new WaitForSeconds(2);

        // Do enemy attack
        AttackType atkType = enemyUnit.weapon.atkType;
        StartCoroutine(EnemyAttack(atkType));

        // TODO more logic for enemy attack
    }

    IEnumerator EnemyAttack(AttackType type)
    {
        (string, int) attackData = damageHandler.NormalAttack(enemyUnit, playerUnit);

        // Sound effect
        animationHandler.PlayDamageAnimation(enemyUnit, playerUnit, sfxSource);

        // Check if player is dead
        bool isPlayerDead = playerUnit.TakeDamage(attackData.Item2);
        uiHandler.SetPlayerHUD(playerUnit);
        uiHandler.DisplayDialogueText(attackData.Item1);
        yield return new WaitForSeconds(2);

        if (isPlayerDead)
        {
            phase = BattlePhase.LOSE;

            // Fade out player
            sfxSource.PlayOneShot(enemyDefeatSFX);
            animationHandler.fadeOutSprite(playerUnit.gameObject, 0.1f);

            StartCoroutine(Lose());
        }
        else
        {
            phase = BattlePhase.PLAYER;
            StartCoroutine(PlayerTurn());
        }
    }

    // State Changes

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
}
