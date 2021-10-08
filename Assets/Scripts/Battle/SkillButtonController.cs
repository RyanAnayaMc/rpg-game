using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text skillName;
    public TMP_Text spCostText;
    public TMP_Text dialogueText;
    public Skill skill;
    private string oldDialogueText;

    // Put button data 
    public void setData()
    {
        skillName.text = skill.skillName;
        spCostText.text = "" + skill.costSP;
    }

    // Puts the skill description in the dialogue box
    public void OnPointerEnter(PointerEventData eventData)
    {
        oldDialogueText = dialogueText.text;
        dialogueText.text = skill.skillDescription;
    }

    // Puts the old dialogue text back
    public void OnPointerExit(PointerEventData eventData)
    {
        dialogueText.text = oldDialogueText;
    }
}
