using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using XNode;

public class NodeParser : MonoBehaviour {
    public DialogueGraph graph;
	public DialogueBoxController dialogueBox;
	private string previousName;

	private Coroutine _parser;

	public void BeginDialogue() {
		StopAllCoroutines();

		_parser = StartCoroutine(ParseNode());
	}

	IEnumerator ParseNode() {
		// Get data for the current node
		BaseNode node = graph.currentNode;
		string dataRaw = node.GetString();
		string[] data = dataRaw.Split('/');

		// Parse the data
		switch (data[0]) {
			case "Start":
				// Start node, lock character movement and setup variables
				previousName = "!@#$%^&#^";
				CharacterMovementController.isPlayerLocked = true;
				Debug.Log("Locked Player");

				NextNode("exit");
				break;
			case "DialogueNode":
				// Dialogue node
				DialogueNode dialogueNode = node as DialogueNode;

				// Display dialogue
				Debug.Log(data[1] + " " + previousName);
				if (data[1] == previousName) {
					// Edit the dialogue if it's the same speaker
					dialogueBox.EditDialogue(data[2], 4);
				} else {
					// If not, display a new dialogue box
					dialogueBox.CloseDialogue();
					yield return new WaitForSeconds((float) 1 / 6);
					dialogueBox.ShowDialouge(data[2], 4, data[1], dialogueNode.GetSprite());
					previousName = data[1];
				}


				yield return new WaitForSeconds(0.5f);
				yield return new WaitUntil(() => Input.GetButtonDown("Interact"));

				if (dialogueBox.isTyping) {
					// If dialogue box is still typing, show the rest of the message
					dialogueBox.ForceFinishText();
					yield return new WaitForSeconds(0.2f);
					yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
				}

				// Go to next node when done typing
				NextNode("exit");
				break;
			case "End":
				// Exit node, go back to beginning node and unlock character
				dialogueBox.CloseDialogue();
				CharacterMovementController.isPlayerLocked = false;
				Debug.Log("Unlocked Player");
				graph.currentNode = graph.startNode;
				break;
		}
	}

	/// <summary>
	/// Parses the next node in the graph.
	/// </summary>
	/// <param name="name">The name of the connection to follow to the next node.</param>
	private void NextNode(string name) {
		if (_parser != null) {
			StopCoroutine(_parser);
			_parser = null;
		}

		foreach (NodePort port in graph.currentNode.Ports) {
			if (port.fieldName == name) {
				graph.currentNode = port.Connection.node as BaseNode;
				break;
			}
		}

		_parser = StartCoroutine(ParseNode());
	}
}
