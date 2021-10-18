using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class SetLocalSwitchNode : BaseNode {

    [Input]
    public int entry;
    [Output]
    public int exit;

    public LocalSwitch switchId;
    public bool switchValue;

    public override string GetString() {
        return "SetLocalSwitchNode/" + switchId.ToString() + "/" + switchValue.ToString();
    }
}