using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BattlePhase
{
    START,
    PLAYER,
    ENEMY,
    WIN,
    LOSE
}

public class BattleController : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    public Text attackButtonText;

    public BattlePhase phase;

    public AudioSource musicSource;
    public AudioClip battleMusic;
    public AudioSource sfxSource;

    public Transform playerLocation;
    public Transform enemyLocation;

    public HUDController playerHUD;
    public HUDController enemyHUD;

    public Text dialogueText;

    Unit playerUnit;
    Unit enemyUnit;

    void Start()
    {
        StartCoroutine(BattleSetup());
    }

    IEnumerator BattleSetup()
    {
        musicSource.clip = battleMusic;
        musicSource.Play();

        phase = BattlePhase.START;
        GameObject playerObj = Instantiate(player, playerLocation);
        GameObject enemyObj = Instantiate(enemy, enemyLocation);
        
        playerObj.transform.position.Set(playerLocation.position.x, playerLocation.position.y, playerLocation.position.z);

        playerUnit = playerObj.GetComponent<Unit>();
        enemyUnit = enemyObj.GetComponent<Unit>();

        playerHUD.SetupHUD(playerUnit);
        enemyHUD.SetupHUD(enemyUnit);

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

        dialogueText.text = "Engaging " + enemyUnit.name + "!";

        yield return new WaitForSeconds(2);

        // Start player turn
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        phase = BattlePhase.PLAYER;
        playerUnit.isDefending = false;
        dialogueText.text = "Choose an action.";
        yield return new WaitForSeconds(0);
    }

    public void OnAttackButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        AttackType atkType = playerUnit.weapon.atkType;
        StartCoroutine(PlayerAttack(atkType));
    }

    public void OnItemButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;
    }

    public void OnDefendButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        StartCoroutine(PlayerDefend());
    }

    IEnumerator PlayerDefend()
    {
        playerUnit.isDefending = true;
        dialogueText.text = playerUnit.name + " defends!";
        yield return new WaitForSeconds(2);
    }

    IEnumerator PlayerAttack(AttackType type)
    {
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
        sfxSource.PlayOneShot(playerUnit.weapon.attackSFX);
        StartCoroutine(FlickerAnimation(enemyUnit));

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

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0);
        AttackType atkType = enemyUnit.weapon.atkType;
        StartCoroutine(EnemyAttack(atkType));
    }

    IEnumerator EnemyAttack(AttackType type)
    {
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
                text = miss ? enemyUnit.name + " missed and dealt no damage!" : playerUnit.name + " took " + damage + " melee damage!";
                break;
            case AttackType.MAGIC:
                damage = miss ? 0 : enemyUnit.mag + enemyUnit.weapon.might - playerUnit.res;
                text = miss ? enemyUnit.name + " missed and dealt no damage!" : playerUnit.name + " took " + damage + " magic damage!";
                break;
            case AttackType.RANGED:
                damage = crit ? enemyUnit.weapon.might - playerUnit.arm : enemyUnit.weapon.might;
                text = crit ? enemyUnit.name + " did " + damage + " critical ranged damage to " + playerUnit.name + "!" : playerUnit.name + " took " + damage + " ranged damage!";
                break;
        }

        if (damage < 0)
        {
            damage = 0;
            text = enemyUnit.name + "'s attack did 0 damage to " + playerUnit.name + "!";
        }

        // Sound effect
        sfxSource.PlayOneShot(enemyUnit.weapon.attackSFX);
        StartCoroutine(FlickerAnimation(playerUnit));

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

    IEnumerator Victory()
    {
        dialogueText.text = "You win!";
        yield return new WaitForSeconds(0);
    }

    IEnumerator Lose()
    {
        dialogueText.text = "You lose...";
        yield return new WaitForSeconds(0);
    }

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
}
