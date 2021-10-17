using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogue : MonoBehaviour {
	public string dialogue;
	public GameObject dialogueBoxObj;
	public Sprite face;
	public string characterName;
	private DialogueBoxController dialogueBox;

	private void Awake() {
		dialogueBox = dialogueBoxObj.GetComponent<DialogueBoxController>();
	}

	public void DisplayDialogue() {
		InteractableEventUtilities.DisplayMessage(dialogueBox,
			new InteractableEventUtilities.Dialogue(characterName, face, dialogue)
		);
	}
}
