using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using XNode;
using System.Threading.Tasks;

#pragma warning disable IDE0051, IDE0060

public class DialogueGraphHandler : MonoBehaviour {
    public DialogueGraph graph;
	public DialogueBoxController dialogueBox;
	[SerializeField] private GameObject[] objects;
	private string previousName;
	private ControlState oldState;

	private Coroutine _parser;

	public void BeginDialogue() {
		StopAllCoroutines();
		graph.currentNode = graph.startNode;

		_parser = StartCoroutine(ParseNode());
	}

	IEnumerator ParseNode() {
		// Get data for the current node
		BaseNode node = graph.currentNode;

		string dataRaw = node.GetString();
		string[] data = dataRaw.Split('/');

		// Performs coroutine based on given data
		yield return StartCoroutine(data[0], data);
	}

	#region Node Handling

	#region Start and End
	// Start node
	private IEnumerator StartNode(string[] data) {
		// Start node, lock character movement and setup variables
		oldState = OnScreenControlsUI.state;
		OnScreenControlsUI.UpdateState(ControlState.Dialogue);
		previousName = "!@#$%^&#^";
		InputMovement.LockPlayer();

		NextNode("exit");
		yield return null;
	}

	// End node
	private IEnumerator EndNode(string[] data) {
		// Exit node, go back to beginning node and unlock character
		dialogueBox.CloseDialogue();
		InputMovement.UnlockPlayer();
		OnScreenControlsUI.UpdateState(oldState);
		graph.currentNode = graph.startNode;
		yield return null;
	}
	#endregion

	#region Conditionals
	// BranchSwitch node
	private IEnumerator BranchSwitch(string[] data) {
		string switchName = data[1];

		bool switchValue = false;

		try {
			switchValue = GlobalSwitches.INSTANCE[switchName];
		} catch (NonExistentSwitchException) {
			GlobalSwitches.INSTANCE.CreateSwitch(switchName, false);
		}

		if (switchValue)
			NextNode("ifTrue");
		else
			NextNode("ifFalse");
		
		yield return null;
	}

	// BranchLocalSwitch node
	private IEnumerator BranchLocalSwitch(string[] data) {
		LocalSwitch localSwitch = (LocalSwitch) Enum.Parse(typeof(LocalSwitch), data[1]);

		yield return null;
		if (graph.GetLocalSwitch(localSwitch))
			NextNode("ifTrue");
		else
			NextNode("ifFalse");
	}

	private IEnumerator BranchVariableAndLiteralNode(string[] data) {
		string variableName = data[1];
		Comparison comparison = (Comparison) Enum.Parse(typeof(Comparison), data[2]);
		int compareValue = Int32.Parse(data[3]);

		int variableValue = GlobalVariables.INSTANCE[variableName];

		bool isTrue = false;

		switch (comparison) {
			case Comparison.IsLessThan:
				isTrue = variableValue < compareValue;
				break;
			case Comparison.IsLessThanOrEqualTo:
				isTrue = variableValue <= compareValue;
				break;
			case Comparison.Equals:
				isTrue = variableValue == compareValue;
				break;
			case Comparison.IsGreaterThan:
				isTrue = variableValue > compareValue;
				break;
			case Comparison.IsGreaterThanOrEqualTo:
				isTrue = variableValue >= compareValue;
				break;
		}

		yield return null;
		if (isTrue)
			NextNode("ifTrue");
		else
			NextNode("ifFalse");
	}

	private IEnumerator BranchVariableAndVariableNode(string[] data) {
		string variable1str = data[1];
		Comparison comparison = (Comparison) Enum.Parse(typeof(Comparison), data[2]);
		string variable2str = data[3];

		int var1 = GlobalVariables.INSTANCE[variable1str];
		int var2 = GlobalVariables.INSTANCE[variable2str];

		bool isTrue = false;

		switch (comparison) {
			case Comparison.IsLessThan:
				isTrue = var1 < var2;
				break;
			case Comparison.IsLessThanOrEqualTo:
				isTrue = var1 <= var2;
				break;
			case Comparison.Equals:
				isTrue = var1 == var2;
				break;
			case Comparison.IsGreaterThan:
				isTrue = var1 > var2;
				break;
			case Comparison.IsGreaterThanOrEqualTo:
				isTrue = var1 >= var2;
				break;
		}

		yield return null;
		if (isTrue)
			NextNode("ifTrue");
		else
			NextNode("ifFalse");
	}

	private IEnumerator BranchDialogueNode(string[] data) {
		BranchDialogueNode branchNode = graph.currentNode as BranchDialogueNode;
		string charName = data[1];
		string dialogueText = data[2];
		bool parseVariables = Boolean.Parse(data[3]);
		string[] options = branchNode.GetOptions();
		Sprite face = branchNode.GetSprite();

		if (parseVariables)
			dialogueText = DialogueBoxController.ParseGlobalVariables(dialogueText);

		// Display dialogue
		if (charName == previousName) {
			// Edit the dialogue if it's the same speaker
			dialogueBox.EditDialogue(dialogueText, 4);
		} else {
			// If not, display a new dialogue box
			dialogueBox.CloseDialogue();
			yield return new WaitForSeconds((float) 1 / 6);
			dialogueBox.ShowDialouge(dialogueText, 4, charName, face);
			previousName = charName;
		}

		// Wait for user input
		yield return new WaitForSeconds(0.5f);
		yield return new WaitUntil(() => (Input.GetButtonDown("Interact") || !dialogueBox.isTyping));

		if (dialogueBox.isTyping) {
			// If dialogue box is still typing, show the rest of the message
			dialogueBox.ForceFinishText();
		}

		// Display the dialogue options
		yield return dialogueBox.StartCoroutine(dialogueBox.ShowOptions(options));

		yield return new WaitUntil(() => dialogueBox.selectedOption >= 0);

		// Go to next node based on option
		NextNode("opt" + dialogueBox.selectedOption);
	}
	#endregion

	#region Setter Fields
	// Switch Set Node
	private IEnumerator SetSwitchNode(string[] data) {
		// Get data
		string switchName = data[1];
		bool switchValue = Boolean.Parse(data[2]);

		// Set the switch
		GlobalSwitches.INSTANCE[switchName] = switchValue;

		NextNode("exit");
		yield return null;
	}
	
	// Local Switch Set Node
	private IEnumerator SetLocalSwitchNode(string[] data) {
		LocalSwitch switchId = (LocalSwitch) Enum.Parse(typeof(LocalSwitch), data[1]);
		bool value = Boolean.Parse(data[2]);

		graph.SetLocalSwitch(switchId, value);

		NextNode("exit");
		yield return null;
	}

	// Variable Set Node
	private IEnumerator SetVariableNode(string[] data) {
		string variable = data[1];
		VariableOperation operation = (VariableOperation) Enum.Parse(typeof(VariableOperation), data[2]);
		float value = float.Parse(data[3]);

		int newValue = GlobalVariables.INSTANCE[variable];
		
		switch (operation) {
			case VariableOperation.ADD:
				newValue = (int) (newValue + value);
				break;
			case VariableOperation.SUBTRACT:
				newValue = (int) (newValue - value);
				break;
			case VariableOperation.MULTIPLY:
				newValue = (int) (newValue * value);
				break;
			case VariableOperation.DIVIDE:
				newValue = (int) (newValue / value);
				break;
			case VariableOperation.REMAINDER:
				newValue = (int) (newValue % value);
				break;
			case VariableOperation.SET:
				newValue = (int) value;
				break;
		}

		GlobalVariables.INSTANCE[variable] = newValue;
		NextNode("exit");
		yield return null;
	}
	#endregion

	#region Dialogue
	// Dialogue node
	private IEnumerator DialogueNode(string[] data) {
		// Dialogue node
		DialogueNode dialogueNode = graph.currentNode as DialogueNode;

		// Get data
		string characterName = data[1];
		string dialogueText = data[2];
		bool parseVariables = Boolean.Parse(data[3]);

		if (parseVariables)
			dialogueText = DialogueBoxController.ParseGlobalVariables(dialogueText);

		// Display dialogue
		if (data[1] == previousName) {
			// Edit the dialogue if it's the same speaker
			dialogueBox.EditDialogue(dialogueText, 4);
		} else {
			// If not, display a new dialogue box
			dialogueBox.CloseDialogue();
			yield return new WaitForSeconds((float) 1 / 6);
			dialogueBox.ShowDialouge(dialogueText, 4, characterName, dialogueNode.GetSprite());
			previousName = characterName;
		}

		// Wait for user input
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
	}

	// Dialogue Bubble Node
	private IEnumerator BubbleNode(string[] data) {
		BubbleNode node = graph.currentNode as BubbleNode;

		BalloonAnimation animation = node.animation;
		Transform target = objects[node.targetObjectIndex].transform;
		int durationMs = node.durationMs;
		bool waitForFinish = node.waitForAnimationToFinish;
		float oX = node.offsetX;
		float oY = node.offsetY;
		float oZ = node.offsetZ;

		BalloonAnimations.INSTANCE.DoAnimationOn(animation, target, durationMs, oX, oY, oZ);

		if (waitForFinish)
			yield return new WaitForSeconds((float) durationMs / 1000);
		else
			yield return null;

		NextNode("exit");
	}

	#endregion

	#region Battle
	// Battle Node
	private IEnumerator BattleNode(string[] data) {
		// Close the dialogue box
		dialogueBox.CloseDialogue();
		OnScreenControlsUI.UpdateState(ControlState.None);
		previousName = "!@#$%^&#^";

		// Get the current node
		BattleNode node = graph.currentNode as BattleNode;

		// Get the battle scene and current scene
		string battleScene = data[1];
		string currentScene = SceneManager.GetActiveScene().name;

		// Get battle parameters
		List<Unit> enemies = node.enemies;
		bool allowDeath = node.allowDeath;
		AudioClip clip = node.battleMusic;

		// Start the battle and wait for it to end
		BattleController.StartBattle(enemies, clip, currentScene, node.allowDeath, battleScene);
		yield return new WaitUntil(() => BattleController.playerWon != null);

		// Determine if player won
		bool victory = (bool) BattleController.playerWon;

		yield return new WaitForSeconds(0.5f);

		OnScreenControlsUI.UpdateState(ControlState.Dialogue);

		// Go to the next node
		if (victory)
			NextNode("victory");
		else
			NextNode("defeat");
	}
	#endregion

	#endregion

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
