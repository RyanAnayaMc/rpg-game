using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SetSwitchNode : BaseNode {
    [Input]
    public int entry;
    [Output]
    public int exit;

    public string switchName;
    public bool switchValue;

    public override string GetString() {
        return "SetSwitchNode/" + switchName + "/" + switchValue.ToString();
	}
}
