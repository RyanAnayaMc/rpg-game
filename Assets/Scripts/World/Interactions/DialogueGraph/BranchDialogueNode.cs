using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BranchDialogueNode : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int opt0;
	[Output]
	public int opt1;
	[Output]
	public int opt2;
	[Output]
	public int opt3;

	public string charName;
	[TextArea]
	public string message;
	public bool parseVariables;
	public Sprite face;

	[TextArea]
	public string[] options;

	public override string GetString() {
		return "BranchDialogueNode/" + charName + "/" + message + "/" + parseVariables.ToString();
	}

	public Sprite GetSprite() {
		return face;
	}

	public string[] GetOptions() {
		return options;
	}
}