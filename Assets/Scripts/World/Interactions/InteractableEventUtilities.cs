using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class InteractableEventUtilities {
	private static bool displayingMessages;

	/// <summary>
	/// Starts a new battle. After the battle the character will return to its spot prior to battle.
	/// </summary>
	/// <param name="enemies">The enemies to spawn in the battle.</param>
	/// <param name="battleMusic">The music to play during the battle.</param>
	/// <param name="combatScene">The combat scene's name.</param>
    public static void StartBattle(List<Unit> enemies, AudioClip battleMusic, string combatScene) {
		BattleController.StartBattle(enemies, battleMusic, SceneManager.GetActiveScene().name, combatScene);
	}

	/// <summary>
	/// Displays all the Dialogues supplied in sequence.
	/// </summary>
	/// <param name="dialogueBox">The scene's DialogueBoxController.</param>
	/// <param name="dialogues">The dialogues to display.</param>
	public static void DisplayMessage(DialogueBoxController dialogueBox, params Dialogue[] dialogues) {
		if (displayingMessages)
			return;
		displayingMessages = true;
		
		dialogueBox.StartCoroutine(showMessages(dialogueBox, dialogues));
	}

	private static IEnumerator showMessages(DialogueBoxController dialogueBox, params Dialogue[] dialogues) {
		CharacterMovementController.isPlayerLocked = true;
		foreach (Dialogue dialogue in dialogues) {
			// Show first message
			dialogueBox.ShowDialouge(dialogue.messages[0], 6, dialogue.name, dialogue.face);
			yield return new WaitForSeconds(0.1f);
			yield return new WaitUntil(() => Input.GetButtonDown("Interact"));

			if (dialogueBox.isTyping) {
				dialogueBox.ForceFinishText();
				yield return new WaitForSeconds(0.1f);
				yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
			}

			Debug.Log("");

			// Show remaining messages
			for (int i = 1; i < dialogue.messages.Length; i++) {
				dialogueBox.EditDialogue(dialogue.messages[i], 6);
				yield return new WaitForSeconds(0.1f);
				yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
				if (dialogueBox.isTyping) {
					dialogueBox.ForceFinishText();
					yield return new WaitForSeconds(0.1f);
					yield return new WaitUntil(() => Input.GetButtonDown("Interact"));
				}
			}

			// Close dialogue box and delay for next iteration
			dialogueBox.CloseDialogue();
			yield return new WaitForSeconds((float) 1 / 3);
		}

		displayingMessages = false;
		CharacterMovementController.isPlayerLocked = false;
	}

	/// <summary>
	/// Small class to hold details about a single dialogue.
	/// </summary>
	public class Dialogue {
		public string[] messages;
		public string name;
		public Sprite face;

		/// <summary>
		/// Creates a new simple dialogue with a message, and optionally a name and
		/// a sprite face.
		/// </summary>
		/// <param name="message">The message to display.</param>
		/// <param name="name">The name to put on the dialogue box.</param>
		/// <param name="face">The face to display near the dialogue box.</param>
		public Dialogue(string message, string name = null, Sprite face = null) {
			this.messages = new string[1];
			messages[0] = message;
			this.name = name;
			this.face = face;
		}

		/// <summary>
		/// Creates a new simple dialogue with multiple messages, and optionally a name and a sprite face.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="face"></param>
		/// <param name="messages"></param>
		public Dialogue(string name = null, Sprite face = null, params string[] messages) {
			this.messages = messages;
			this.name = name;
			this.face = face;
		}
	}
}
