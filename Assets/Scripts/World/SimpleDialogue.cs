using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SimpleDialogue : MonoBehaviour {
	public string dialogue;
	public GameObject dialogueBoxObj;
	private DialogueBoxController dialogueBox;
	Collider charCollider;
	private bool isTriggered;

	private void Awake() {
		charCollider = GetComponent<Collider>();
		dialogueBox = dialogueBoxObj.GetComponent<DialogueBoxController>();
		isTriggered = false;

		displayDialogue();
	}

	private void FixedUpdate() {
		if (isTriggered && Input.GetAxis("Interact") > 0.01)
			displayDialogue();
	}

	private void displayDialogue() {
		dialogueBox.ShowDialouge(dialogue, 6);
		while (Input.GetAxis("Interact") < 0.01);
		dialogueBox.CloseDialogue();
	}

	private void OnTriggerEnter(Collider other) {
		Debug.Log(isTriggered);
		isTriggered = true;
	}

	private void OnTriggerExit(Collider other) {
		Debug.Log(isTriggered);
		isTriggered = false;
	}
}
