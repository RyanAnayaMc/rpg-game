using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBoxController : MonoBehaviour
{
    public TMP_Text Text;
    private Animator animator;
    private string currentText;
    private const string kAlpha = "<color=00000000>";
    private float speed;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", false);
    }

    /// <summary>
    /// Shows a dialogue box with the given text.
    /// </summary>
    /// <param name="text">The text to display in the text box.</param>
    /// <param name="spd">Text display speed in letters per second.</param>
    public void ShowDialouge(string text, float spd) {
        animator.SetBool("isOpen", true);
        currentText = text;
        speed = spd;
	}

    /// <summary>
    /// Edits the dialogue on the text window without erasing it.
    /// </summary>
    /// <param name="text">The text to display in the text box.</param>
    /// <param name="spd">Text display speed in letters per second.</param>
    public void EditDialogue(string text, float spd) {
        if (!animator.GetBool("isOpen"))
            return;

        currentText = text;
        speed = spd;

        StartCoroutine(displayText());
	}

    /// <summary>
    /// Closes the dialogue box.
    /// </summary>
    public void CloseDialogue() {
        animator.SetBool("isOpen", false);
    }

    public void OnDialogueOpen() {
        StartCoroutine(displayText());
    }

    public void OnDialogueClose() {
        StopAllCoroutines();
    }

    private IEnumerator displayText() {
        Text.text = "";

        string originalText = currentText;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in currentText.ToCharArray()) {
            alphaIndex++;
            Text.text = originalText;
            displayedText = Text.text.Insert(alphaIndex, kAlpha);

            Text.text = displayedText;

            yield return new WaitForSeconds(1 / speed);
		}
	}
}
