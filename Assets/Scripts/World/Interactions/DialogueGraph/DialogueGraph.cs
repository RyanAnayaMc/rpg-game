using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public enum LocalSwitch {
	A = 0, B = 1, C = 2, D = 3, E = 4, F = 5
}


[CreateAssetMenu(fileName = "NewDialogueGraph", menuName = "Dialogue/DialogueGraph")]
public class DialogueGraph : NodeGraph {
	private BaseNode _startNode;
	private BaseNode _currentNode;
	private bool[] localSwitches = new bool[6];

	public bool GetLocalSwitch(LocalSwitch localSwitch) {
		return localSwitches[(int) localSwitch];
	}

	public void SetLocalSwitch(LocalSwitch localSwitch, bool value) {
		localSwitches[(int) localSwitch] = value;
	}

	public BaseNode currentNode {
		get {
			if (_currentNode == null)
				_currentNode = startNode;

			return _currentNode;
		}
		set { _currentNode = value; }
	}

	public BaseNode startNode {
		get {
			if (_startNode == null) {
				foreach (BaseNode node in nodes) {
					if (node.GetString() == "StartNode") {
						_startNode = node;
						break;
					}
				}
			}

			return _startNode;
		}
	}


}