using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int exit;

	public string characterName;
	public string message;
	public Sprite face;

	public override string GetString() {
		return "DialogueNode/" + characterName + "/" + message;
	}

	public Sprite GetSprite() {
		return face;
	}
}