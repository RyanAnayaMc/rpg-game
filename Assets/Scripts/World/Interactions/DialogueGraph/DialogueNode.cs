using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int exit;

	public string charName;
	[TextArea]
	public string message;
	public bool parseVariables;
	public Sprite face;

	public override string GetString() {
		return "DialogueNode/" + charName + "/" + message + "/" + parseVariables.ToString();
	}

	public Sprite GetSprite() {
		return face;
	}
}