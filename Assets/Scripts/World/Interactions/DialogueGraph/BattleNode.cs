using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BattleNode : BaseNode {
	[Input] public int entry;
	[Output] public int victory;
	[Output] public int defeat;

	public bool allowDeath;
	public string battleScene;
	public List<Unit> enemies;
	public AudioClip battleMusic;

	public override string GetString() {
		return "BattleNode/" + battleScene;
	}
}
