using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStart : MonoBehaviour {
    public PlayerUnit playerUnit;
    public List<Unit> enemies;
    public string combatScene;
    public AudioClip battleMusic;
    public bool allowLose;

    public void StartBattle() {
        BattleController.StartBattle(enemies, battleMusic, gameObject.scene.name, allowLose, combatScene);
    }
}
