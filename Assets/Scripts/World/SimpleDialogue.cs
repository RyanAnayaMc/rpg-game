using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDialogue : MonoBehaviour {
	public string dialogue;
	public GameObject dialogueBoxObj;
	public Sprite face;
	public string characterName;
	private DialogueBoxController dialogueBox;
	private bool isDisplayed = false;

	private void Awake() {
		dialogueBox = dialogueBoxObj.GetComponent<DialogueBoxController>();
	}

	public void DisplayDialogue() {
		if (isDisplayed)
			return;
		isDisplayed = true;
		CharacterMovementController.isPlayerLocked = true;
		StartCoroutine(displayDialogue());
	}

	private IEnumerator displayDialogue() {
		dialogueBox.ShowDialouge(dialogue, 6, characterName, face);
		yield return new WaitForSeconds(0.5f);

		yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
		if (dialogueBox.isTyping) {
			dialogueBox.ForceFinishText();
			yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
		}

			dialogueBox.CloseDialogue();
		yield return new WaitForSeconds(1);
		isDisplayed = false;
		CharacterMovementController.isPlayerLocked = false;
	}

	public void DebugMessage() {
		Debug.Log(dialogue);
	}
}
