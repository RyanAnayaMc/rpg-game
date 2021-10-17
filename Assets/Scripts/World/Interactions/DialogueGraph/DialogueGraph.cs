using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class DialogueGraph : NodeGraph {
	private BaseNode _startNode;
	private BaseNode _currentNode;

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
					if (node.GetString() == "Start") {
						_startNode = node;
						break;
					}
				}
			}

			return _startNode;
		}
	}


}