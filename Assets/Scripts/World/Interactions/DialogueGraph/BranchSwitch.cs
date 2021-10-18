using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BranchSwitch : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int ifTrue;
	[Output]
	public int ifFalse;
	
	public string switchName;

	public override string GetString() {
		return "BranchSwitch/" + switchName;
	}
}