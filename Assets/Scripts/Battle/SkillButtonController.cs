using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text skillName;
    [SerializeField]
    private TMP_Text spCostText;
    public TMP_Text dialogueText;
    public Skill skill;
    private string oldDialogueText = "";

    // Put button data 
    public void setData(Skill skill)
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

    // Makes the sp cost text red
    public void RedSPCostText()
    {
        spCostText.color = new Color(1, 0, 0);
    }

    // Makes the sp cost text white
    public void WhiteSPCostText()
    {
        spCostText.color = new Color(1, 1, 1);
    }
}
