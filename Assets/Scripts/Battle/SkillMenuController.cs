using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform content; // The transform for the content field for the Skills menu
    [SerializeField]
    private GameObject skillButtonPrefab; // The prefab for the Skill button
    [SerializeField]
    private TMP_Text dialogueBoxText; // The dialogue box text on the battle screen
    [SerializeField]
    private BattleController battleController; // The battle controller for the battle

    private List<Skill> playerSkills;
    private List<SkillButtonController> skillButtons;

    public void SetSkillMenu()
    {
        if (skillButtons == null)
            PopulateSkillMenu();
        else
            UpdateSkillMenu();
    }

    public void UpdateSkillMenu()
    {
        foreach (SkillButtonController button in skillButtons)
        {
            if (battleController.playerUnit.unit.cSP < button.skill.costSP)
                button.RedSPCostText();
            else
                button.WhiteSPCostText();
        }
    }

    public void PopulateSkillMenu()
    {
        playerSkills = battleController.playerUnit.unit.skills;

        skillButtons = new List<SkillButtonController>();

        for (int i = 0; i < playerSkills.Count; i++) {
            // Get the skill and instantiate the button
            Skill skill = playerSkills[i];
            GameObject skillButton = Instantiate(skillButtonPrefab, content);

            // Setup the text on the button
            SkillButtonController buttonController = skillButton.GetComponent<SkillButtonController>();
            skillButtons.Add(buttonController);
            buttonController.dialogueText = dialogueBoxText;
            buttonController.skill = skill;
            buttonController.setData(skill);
            if (battleController.playerUnit.unit.cSP < skill.costSP)
                buttonController.RedSPCostText();

            // Register the listener for the button
            Button button = skillButton.GetComponent<Button>();
            int j = i;
            button.onClick.AddListener(() => battleController.onSkillButton(j));
            button.name = i.ToString();
        }
    }
}
