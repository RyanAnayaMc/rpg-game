using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BranchVariableAndVariableNode : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int ifTrue;
	[Output]
	public int ifFalse;

	public string variable1;
	public Comparison comparison;
	public string variable2;

	public override string GetString() {
		return "BranchVariableAndVariableNode/" + variable1 + "/" + comparison.ToString() + "/" + variable2;
	}
}