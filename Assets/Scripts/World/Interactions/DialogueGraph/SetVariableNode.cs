using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum VariableOperation {
	ADD,
	SUBTRACT,
	MULTIPLY,
	DIVIDE,
	REMAINDER,
	SET
}
public class SetVariableNode : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int exit;

	public string variable;
	public VariableOperation operation;
	public float value;

	public override string GetString() {
		return "SetVariableNode/" + variable + "/" + operation.ToString() + "/" + value;
	}
}