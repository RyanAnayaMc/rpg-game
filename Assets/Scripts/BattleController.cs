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

public enum AttackType
{
    MELEE,
    MAGIC,
    RANGED
}

public class BattleController : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    public BattlePhase phase;

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
        phase = BattlePhase.START;
        GameObject playerObj = Instantiate(player, playerLocation);
        GameObject enemyObj = Instantiate(enemy, enemyLocation);

        playerObj.transform.position.Set(playerLocation.position.x, playerLocation.position.y, playerLocation.position.z);

        playerUnit = playerObj.GetComponent<Unit>();
        enemyUnit = enemyObj.GetComponent<Unit>();

        playerHUD.SetupHUD(playerUnit);
        enemyHUD.SetupHUD(enemyUnit);

        dialogueText.text = "Engaging " + enemyUnit.name + "!";

        yield return new WaitForSeconds(3);

        // Start player turn
        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn()
    {
        phase = BattlePhase.PLAYER;
        dialogueText.text = "Choose an action.";

        yield return new WaitForSeconds(2);
    }

    public void OnMeleeButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        StartCoroutine(PlayerAttack(AttackType.MELEE));
    }

    public void OnCastButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        StartCoroutine(PlayerAttack(AttackType.MAGIC));
    }

    public void OnFireButton()
    {
        if (phase != BattlePhase.PLAYER)
            return;

        StartCoroutine(PlayerAttack(AttackType.RANGED));
    }

    IEnumerator PlayerAttack(AttackType type)
    {
        yield return new WaitForSeconds(2);

        int damage = 0;
        float accuracy = (playerUnit.dex > enemyUnit.agi) ? 100 : 100 - (enemyUnit.agi - playerUnit.dex) / playerUnit.dex;
        float crit = (playerUnit.dex - enemyUnit.agi) / playerUnit.dex;

        switch (type)
        {
            case AttackType.MELEE:
                damage = (Random.value * 100 > accuracy) ? 0 : playerUnit.str - enemyUnit.def;
                break;
            case AttackType.MAGIC:
                damage = (Random.value * 100 > accuracy) ? 0 : playerUnit.mag - enemyUnit.res;
                break;
            case AttackType.RANGED:
                damage = (Random.value * 100 > crit) ? playerUnit.dex - enemyUnit.arm : playerUnit.dex; // TODO fix to use weapon MT instead of dex
                break;
        }

        enemyUnit.TakeDamage(damage);
        enemyHUD.SetHP(enemyUnit);
        dialogueText.text = enemyUnit.name + " took " + damage + " " + type + " damage!";
        yield return new WaitForSeconds(3);
    }
}
