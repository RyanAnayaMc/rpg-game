using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Text attackButtonText; // The text on the "Attack" button

    BattlePhase phase; // Current phase of the battle

    public AudioSource musicSource; // The Scene's music player
    public AudioClip battleMusic; // The music to play during the battle
    public AudioClip victoryFanfare; // Fanfare that plays if you win
    public AudioClip victoryMusic; // Music that plays after the fanfare
    public AudioSource sfxSource; // The scene's sound effect player

    public Transform playerLocation; // The location to place the player
    public Transform enemyLocation; // The location to place the enemy

    public HUDController playerHUD; // The location of the HUD that shows the player's info
    public HUDController enemyHUD; // The location of the HUD that shows the enemy's info

    public GameObject playerPhaseUI; // The GameObject to display at the beginning of Player Phase
    public GameObject enemyPhaseUI; // The GameObject to display at the beginning of Enemy Phase

    public Text dialogueText; // The text on the dialogue box

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
        musicSource.clip = battleMusic;
        musicSource.loop = true;
        musicSource.Play();

        // Spawn player and enemy
        phase = BattlePhase.START;
        GameObject playerObj = Instantiate(player, playerLocation);
        GameObject enemyObj = Instantiate(enemy, enemyLocation);

        playerUnit = playerObj.GetComponent<Unit>();
        enemyUnit = enemyObj.GetComponent<Unit>();

        // Load HUD
        playerHUD.SetupHUD(playerUnit);
        enemyHUD.SetupHUD(enemyUnit);

        // Update text on attack button based on equipped weapon
        AttackType atkType = playerUnit.weapon.atkType;
        switch (atkType)
        {
            case AttackType.MELEE:
                attackButtonText.text = "Melee";
                break;
            case AttackType.MAGIC:
                attackButtonText.text = "Cast";
                break;
            case AttackType.RANGED:
                attackButtonText.text = "Fire";
                break;
        }

        // Enemy spawn text
        dialogueText.text = "Engaging " + enemyUnit.name + "!";

        yield return new WaitForSeconds(2);

        // Start player turn
        StartCoroutine(PlayerTurn());
    }

    // Player Actions
    IEnumerator PlayerTurn()
    {
        // Show player phase image
        StartCoroutine(fadeIn(playerPhaseUI));
        yield return new WaitForSeconds(1);
        StartCoroutine(fadeOut(playerPhaseUI));

        yield return new WaitForSeconds(1);

        // Let player pick an option
        phase = BattlePhase.PLAYER;
        playerUnit.isDefending = false;
        dialogueText.text = "Choose an action.";
        
    }

    // Player clicks attack button
    public void OnAttackButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;
        phase = BattlePhase.PLAYER_ACTION;

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
    }

    // Player clicks defend button
    public void OnDefendButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;
        phase = BattlePhase.PLAYER_ACTION;
        StartCoroutine(PlayerDefend());
        StartCoroutine(EnemyTurn());
    }

    // Make the player defend themselves
    IEnumerator PlayerDefend()
    {
        playerUnit.isDefending = true;
        dialogueText.text = playerUnit.name + " defends!";
        yield return new WaitForSeconds(2);
    }

    // Handles player attacking
    IEnumerator PlayerAttack(AttackType type)
    {
        // Calculate damage
        int damage = 0;
        float accuracy = (playerUnit.dex > enemyUnit.agi) ? 100 : 100 - (enemyUnit.agi - playerUnit.dex) / playerUnit.dex;
        float critChance = (playerUnit.dex - enemyUnit.agi) / playerUnit.dex;
        bool miss = Random.value * 100 > accuracy;
        bool crit = Random.value * 100 > critChance;
        string text = "";

        switch (type)
        {
            case AttackType.MELEE:
                damage = miss ? 0 : playerUnit.str + playerUnit.weapon.might - enemyUnit.def;
                text = miss ? playerUnit.name + " missed and dealt no damage!" : enemyUnit.name + " took " + damage + " melee damage!";
                break;
            case AttackType.MAGIC:
                damage = miss ? 0 : playerUnit.mag + playerUnit.weapon.might - enemyUnit.res;
                text = miss ? playerUnit.name + " missed and dealt no damage!" : enemyUnit.name + " took " + damage + " magic damage!";
                break;
            case AttackType.RANGED:
                damage = crit ? playerUnit.weapon.might - enemyUnit.arm : playerUnit.weapon.might;
                text = crit ? playerUnit.name + " did " + damage + " critical ranged damage to " + enemyUnit.name + "!" : enemyUnit.name + " took " + damage + " ranged damage!";
                break;
        }

        if (damage < 0)
        {
            damage = 0;
            text = playerUnit.name + "'s attack did 0 damage to " + enemyUnit.name + "!";
        }

        // Sound effect and animation
        StartCoroutine(PlayDamageAnimation(playerUnit, enemyUnit, sfxSource));

        // Check if enemy is dead
        bool isEnemyDead = enemyUnit.TakeDamage(damage);
        enemyHUD.SetHP(enemyUnit);
        dialogueText.text = text;
        yield return new WaitForSeconds(2);

        if (isEnemyDead)
        {
            phase = BattlePhase.WIN;
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
        // Show enemy phase image
        StartCoroutine(fadeIn(enemyPhaseUI));
        phase = BattlePhase.ENEMY;
        yield return new WaitForSeconds(1);
        StartCoroutine(fadeOut(enemyPhaseUI));
        yield return new WaitForSeconds(1);

        // Do enemy attack
        AttackType atkType = enemyUnit.weapon.atkType;
        StartCoroutine(EnemyAttack(atkType));

        // TODO more logic for enemy attack
    }

    IEnumerator EnemyAttack(AttackType type)
    {
        // Calculate damage
        int damage = 0;
        float accuracy = (enemyUnit.dex > playerUnit.agi) ? 100 : 100 - (playerUnit.agi - enemyUnit.dex) / enemyUnit.dex;
        float critChance = (enemyUnit.dex - playerUnit.agi) / enemyUnit.dex;
        bool miss = Random.value * 100 > accuracy;
        bool crit = Random.value * 100 > critChance;
        string text = "";

        switch (type)
        {
            case AttackType.MELEE:
                damage = miss ? 0 : enemyUnit.str + enemyUnit.weapon.might - playerUnit.def;
                if (playerUnit.isDefending)
                    damage /= 2;
                text = miss ? enemyUnit.name + " missed and dealt no damage!" : playerUnit.name + " took " + damage + " melee damage!";
                break;
            case AttackType.MAGIC:
                damage = miss ? 0 : enemyUnit.mag + enemyUnit.weapon.might - playerUnit.res;
                if (playerUnit.isDefending)
                    damage /= 2;
                text = miss ? enemyUnit.name + " missed and dealt no damage!" : playerUnit.name + " took " + damage + " magic damage!";
                break;
            case AttackType.RANGED:
                damage = crit ? enemyUnit.weapon.might - playerUnit.arm : enemyUnit.weapon.might;
                if (playerUnit.isDefending)
                    damage /= 2;
                text = crit ? enemyUnit.name + " did " + damage + " critical ranged damage to " + playerUnit.name + "!" : playerUnit.name + " took " + damage + " ranged damage!";
                break;
        }

        if (damage < 0)
        {
            damage = 0;
            text = enemyUnit.name + "'s attack did 0 damage to " + playerUnit.name + "!";
        }

        // Sound effect
        StartCoroutine(PlayDamageAnimation(enemyUnit, playerUnit, sfxSource));

        // Check if player is dead
        bool isPlayerDead = playerUnit.TakeDamage(damage);
        playerHUD.SetHP(playerUnit);
        dialogueText.text = text;
        yield return new WaitForSeconds(2);

        if (isPlayerDead)
        {
            phase = BattlePhase.LOSE;
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
        dialogueText.text = "You win!";
        musicSource.Stop();
        StartCoroutine(PlayVictoryFanfare());
        
        yield return new WaitForSeconds(0);

        // TODO put player back to map screen
    }

    // Plays the victory fanfare sound, then victory music after
    IEnumerator PlayVictoryFanfare()
    {
        // Play victory fanfare
        musicSource.loop = false;
        musicSource.clip = victoryFanfare;
        musicSource.Play();

        // Wait for it to finish
        while (musicSource.isPlaying)
            yield return new WaitForSeconds(0.5f);

        // Play victory music
        musicSource.clip = victoryMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Player lost
    IEnumerator Lose()
    {
        // TODO make game over screen
        dialogueText.text = "You lose...";
        yield return new WaitForSeconds(0);
    }

    // Battle Animations

    // Makes a Unit flash multiple times
    IEnumerator FlickerAnimation(Unit unit)
    {
        SpriteRenderer sr = unit.gameObject.GetComponent<SpriteRenderer>();
        Color oldColor = sr.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, 0);

        for (int i = 0; i < 5; i++)
        {
            sr.color = newColor;
            yield return new WaitForSeconds(0.1f);
            sr.color = oldColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Plays a cast and damage animation, including sound effects
    public IEnumerator PlayDamageAnimation(Unit attackingUnit, Unit targetUnit, AudioSource sfxSource)
    {
        // Play cast sound effect and animation
        Weapon weapon = attackingUnit.weapon;
        if (weapon.castSFX != null)
            sfxSource.PlayOneShot(weapon.castSFX);
        attackingUnit.effectRenderer.GetComponent<Animator>().SetTrigger(weapon.castAnimation.ToString());

        // Play attack sound effect and animation
        targetUnit.effectRenderer.GetComponent<Animator>().SetTrigger(weapon.weaponAnimation.ToString());
        if (weapon.attackSFX != null)
            sfxSource.PlayOneShot(weapon.attackSFX);

        // Play flicker animation
        StartCoroutine(FlickerAnimation(targetUnit));

        yield return new WaitForSeconds(0);
    }

    // Makes a GameObject with an Image component fade in
    IEnumerator fadeIn(GameObject obj)
    {
        Color color = obj.GetComponent<Image>().color;
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float a = 0;

        while (a < 1)
        {
            a += 0.03f;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }
    
    // Makes a GameObject with an Image component fade out
    IEnumerator fadeOut(GameObject obj)
    {
        Color color = obj.GetComponent<Image>().color;
        float r = color.r;
        float g = color.g;
        float b = color.b;
        float a = 1;

        while (a > 0)
        {
            a -= 0.03f;
            obj.GetComponent<Image>().color = new Color(r, g, b, a);
            yield return new WaitForFixedUpdate();
        }
    }
}
