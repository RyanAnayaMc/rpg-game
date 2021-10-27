using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

#pragma warning disable IDE0044

public class SkillButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public TMP_Text elementName;
    [SerializeField]
    private TMP_Text elementNumberText;
    [SerializeField]
    private Image icon;
    public DialogueBoxController dialogueBox;
    public TMP_Text textField;
    public bool useTextField;
    public ButtonTextable element;
    private string oldDialogueText = "";

    /// <summary>
    /// Puts the skill icon, name, and SP cost on the button text.
    /// </summary>
    /// <param name="skill">The skill to use for the data.</param>
    public void SetData(ButtonTextable textable) {
        element = textable;
        elementName.text = textable.GetName();
        elementNumberText.text = textable.GetNumber().ToString();
        if (textable.GetIcon() == null)
            icon.color = Color.clear;
		else 
            icon.sprite = textable.GetIcon();
    }

	/// <summary>
	/// Puts the skill description in the dialogue box when mousing over the skill.
	/// </summary>
	public void OnPointerEnter(PointerEventData eventData) {
        if (useTextField) {
            oldDialogueText = textField.text;
            textField.text = element.GetDescriptionText();
        } else {
            oldDialogueText = dialogueBox.Text.text;
            dialogueBox.EditDialogue(element.GetDescriptionText(), 3);
        }
    }

    /// <summary>
    /// Puts the old dialogue text back when moving the mouse off the skill.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData) {
        if (useTextField)
            textField.text = oldDialogueText;
		else 
            dialogueBox.EditDialogue(oldDialogueText, 3);
    }

    /// <summary>
    /// Makes the number text red.
    /// </summary>
    public void RedNumber() {
        elementNumberText.color = new Color(1, 0, 0);
    }

    /// <summary>
    /// Makes the number text white.
    /// </summary>
    public void WhiteNumber() {
        elementNumberText.color = new Color(1, 1, 1);
    }
}
