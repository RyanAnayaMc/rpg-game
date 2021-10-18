using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum Comparison {
	IsLessThan,
	IsLessThanOrEqualTo,
	Equals,
	IsGreaterThan,
	IsGreaterThanOrEqualTo
}
public class BranchVariableAndLiteralNode : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int ifTrue;
	[Output]
	public int ifFalse;

	public string variableName;
	public Comparison comparison;
	public int compareValue;

	public override string GetString() {
		return "BranchVariableAndLiteralNode/" + variableName + "/" + comparison.ToString() + "/" + compareValue;
	}
}