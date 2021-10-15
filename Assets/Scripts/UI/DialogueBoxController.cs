using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBoxController : MonoBehaviour
{
    public TMP_Text Text;
    private Animator animator;
    private string currentText;
    private string originalText;
    private const string kAlpha = "<color=#00000000>";
    private int speed;
    public bool isTyping = false;
    public Image face;
    public string characterName;
    public TMP_Text characterNameText;

    void Awake()
    {
        transform.localScale = new Vector3(1, 0, 1);
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", false);
    }

    /// <summary>
    /// Shows a dialogue box with the given text.
    /// </summary>
    /// <param name="text">The text to display in the text box.</param>
    /// <param name="spd">Text display speed in frames per letter.</param>
    public void ShowDialouge(string text, int spd, string name = null, Sprite characterFace = null) {
        if (name == null)
            characterNameText.gameObject.SetActive(false);
        else {
            characterNameText.gameObject.SetActive(true);
            characterNameText.text = name;
		}

        if (characterFace == null)
            face.color = new Color(0, 0, 0, 0);
		else {
            face.color = new Color(1, 1, 1, 1);
            face.sprite = characterFace;
		}

        if (animator.GetBool("isOpen")) {
            Debug.Log("is Open");
            return;
        }
        Text.text = "";
        animator.SetBool("isOpen", true);
        currentText = text;
        speed = spd;
	}

    /// <summary>
    /// Edits the dialogue on the text window without erasing it.
    /// </summary>
    /// <param name="text">The text to display in the text box.</param>
    /// <param name="spd">Text display speed in frames per letter.</param>
    public void EditDialogue(string text, int spd) {
        if (!animator.GetBool("isOpen"))
            return;

        currentText = text;
        speed = spd;

        StopAllCoroutines();

        // Finish typing if it was busy
        if (isTyping)
            Text.text = currentText;

        StartCoroutine(displayText());
	}

    /// <summary>
    /// Closes the dialogue box.
    /// </summary>
    public void CloseDialogue() {
        animator.SetBool("isOpen", false);
    }

    public void OnDialogueOpen() {
        StopAllCoroutines();
        StartCoroutine(displayText());
    }

    public void OnDialogueClose() {
        StopAllCoroutines();
    }

    private IEnumerator displayText() {
        Text.text = "";

        isTyping = true;

        originalText = stripHTML(currentText);
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in originalText.ToCharArray()) {
            alphaIndex++;
            string text = originalText;
            displayedText = text.Insert(alphaIndex, kAlpha);

            Text.text = displayedText;

            for (int i = 0; i < speed; i++)
                yield return new WaitForEndOfFrame();
		}

        isTyping = false;
	}

    /// <summary>
    /// If text is still typing, automatically finishes the message
    /// </summary>
    public void ForceFinishText() {
        if (isTyping) {
            StopAllCoroutines();
            Text.text = originalText;
            isTyping = false;
		}
	}

    /// <summary>
    /// Removes any characters betweeen angle brackets and the angle brackets themselves
    /// from a string.
    /// </summary>
    /// <returns>String with HTML stripped out.</returns>
    private string stripHTML(string str) {
        int index1 = -1;
        int index2 = -1;

        for (int i = 0; i < str.Length; i++) {
            if (str[i] == '<') {
                index1 = i;
                break;
            }
        }

        if (index1 < 0) {
            return str;
        }

        for (int i = index1; i < str.Length; i++) {
            if (str[i] == '>') {
                index2 = i;
                break;
            }
        }

        if (index1 < 0 || index2 < 1 || index1 > index2)
            return str;

        string newStr = str.Substring(0, index1) + str.Substring(index2 + 1);

        return stripHTML(newStr);
    }
}
