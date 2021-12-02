using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#pragma warning disable IDE0051

public class DialogueBoxController : MonoBehaviour {
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
    public GameObject dialogueSelectionWindow;
    public GameObject dialogueButtonPrefab;

    public int selectedOption;

    void Awake()
    {
        if (dialogueSelectionWindow != null)
            dialogueSelectionWindow.GetComponent<Animator>().SetBool("isOpen", false);
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

        if (animator.GetBool("isOpen")) return;

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

        StartCoroutine(DisplayText());
	}

    /// <summary>
    /// Closes the dialogue box.
    /// </summary>
    public void CloseDialogue() {
        animator.SetBool("isOpen", false);
    }

    /// <summary>
    /// Once the dialogue box is open, displays the text
    /// </summary>
    public void OnDialogueOpen() {
        StopAllCoroutines();
        StartCoroutine(DisplayText());
    }

    /// <summary>
    /// Once the dialogue box closes, halts all dialogue coroutines
    /// </summary>
    public void OnDialogueClose() {
        StopAllCoroutines();
    }

    /// <summary>
    /// Displays the supplied text on the dialogue box.
    /// </summary>
    private IEnumerator DisplayText() {
        // Initialize local variables
        Text.text = "";
        isTyping = true;

        // Remove HTML tags (i.e. alpha tag) from current text
        originalText = StripHTML(currentText);

        // Display the text on the dialogue box
        if (Settings.INSTANCE.useTextAnimation) {
            // Display text on the dialogue box with dialogue animation
            string displayedText;
            int alphaIndex = 0;

            // Display each character one at a time
            foreach (char c in originalText.ToCharArray()) {
                alphaIndex++;
                string text = originalText;
                displayedText = text.Insert(alphaIndex, kAlpha);

                Text.text = displayedText;

                for (int i = 0; i < speed; i++)
                    yield return new WaitForEndOfFrame();
            }
        } else Text.text = originalText; // Displays the text on the dialogue box all at once

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
    private string StripHTML(string str) {
        int index1 = -1;
        int index2 = -1;

        // Find opening angle bracket
        for (int i = 0; i < str.Length; i++) {
            if (str[i] == '<') {
                index1 = i;
                break;
            }
        }

        // Return if it wasn't found
        if (index1 < 0) {
            return str;
        }

        // Find closing angle bracket
        for (int i = index1; i < str.Length; i++) {
            if (str[i] == '>') {
                index2 = i;
                break;
            }
        }

        // Return if it wasn't found
        if (index1 < 0 || index2 < 1 || index1 > index2)
            return str;

        // It was found, so remove the HTML tag
        string newStr = str.Substring(0, index1) + str.Substring(index2 + 1);

        // See if thre are any more HTML tags present
        return StripHTML(newStr);
    }

    /// <summary>
    /// Finds any global variable references in the string and puts the variables in.
    /// Reference global variables like this:
    /// {variableName}
    /// For example, if the variable EnemiesKilled is 8, then this string:
    /// "You killed {EnemiesKilled} enemies!"
    /// as input would return:
    /// "You killed 8 enemies!"
    /// </summary>
    /// <param name="str">The string to parse variables for.</param>
    /// <returns>The parsed string.</returns>
    public static string ParseGlobalVariables(string str) {
        // Find curly braces
        int index1 = str.IndexOf('{');
        int index2 = str.IndexOf('}');

        // If curly braces were not found or were invalud, return
        if (index1 < 0 || index2 < 0 || index2 < index1)
            return str;

        // Curly braces were found, extract variable name and get its value
        string varName = str.Substring(index1 + 1, index2 - index1 - 1);
        int varValue = GlobalVariables.INSTANCE[varName];

        // Insert variable value to the message
        string newString = str.Substring(0, index1) + varValue + str.Substring(index2 + 1);

        // Find more variables
        return ParseGlobalVariables(newString);
	}

    /// <summary>
    /// Shows the given dialouge options, up to four of them. Saves result in
    /// selectedOption field. It is set to -1 while waiting for the player to answer.
    /// </summary>
    /// <param name="options">The</param>
    public IEnumerator ShowOptions(params string[] options) {
        // Determine how many options there are
        int optionCount = Mathf.Min(4, options.Length);

        // Create array of the possible options
        GameObject[] optionButtons = new GameObject[optionCount];

        // Animate the opening of the options
        Animator animator = dialogueSelectionWindow.GetComponent<Animator>();
        animator.SetBool("isOpen", true);
        int buttonPressed = -1;

        // Add the buttons to the options menu
        for (int i = 0; i < optionCount; i++) {
            optionButtons[i] = Instantiate(dialogueButtonPrefab, dialogueSelectionWindow.transform);
            optionButtons[i].GetComponentInChildren<TMP_Text>().text = options[i];
            int j = i;
            optionButtons[i].GetComponent<Button>().onClick.
                AddListener(() => {
                    if (buttonPressed < 0)
                        buttonPressed = j;
			    });
        }

        // Wait until the user picks one
        yield return new WaitUntil(() => buttonPressed >= 0);

        // Close the options menu
        animator.SetBool("isOpen", false);

        // Destroy the instantiated objects
        foreach (GameObject obj in optionButtons)
            Destroy(obj);

        // Save output value
        selectedOption = buttonPressed;
	}
}
