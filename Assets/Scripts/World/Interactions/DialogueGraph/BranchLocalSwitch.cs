using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class BranchLocalSwitch : BaseNode {
	[Input]
	public int entry;
	[Output]
	public int ifTrue;
	[Output]
	public int ifFalse;
	public LocalSwitch localSwitch;

	public override string GetString() {
		return "BranchLocalSwitch/" + localSwitch.ToString();
	}
}