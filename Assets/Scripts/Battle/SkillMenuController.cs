using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillMenuController : MonoBehaviour {
    /// <summary>
    /// Reference to the battle's BattleController for convenience.
    /// </summary>
    [HideInInspector]
    public BattleController battleController;

    [SerializeField]
    private Transform content; // The transform for the content field for the Skills menu
    [SerializeField]
    private GameObject skillButtonPrefab; // The prefab for the Skill button
    [SerializeField]
    private DialogueBoxController dialogueBox; // The dialogue box on the battle screen

    private List<Skill> playerSkills;
    private List<SkillButtonController> skillButtons;

    /// <summary>
    /// Populates the skill menu if not populated. Otherwise, updates it.
    /// </summary>
    public void SetSkillMenu() {
        if (skillButtons == null)
            PopulateSkillMenu();
        else
            UpdateSkillMenu();
    }

    // Updates the skill menu. Makes SP text red if unit does not have enough SP,
    // or white otherwise.
    private void UpdateSkillMenu() {
        foreach (SkillButtonController button in skillButtons) {
            if (battleController.playerUnit.unit.cSP < button.element.GetNumber())
                button.RedNumber();
            else
                button.WhiteNumber();
                    ;
        }
    }

    // Puts all the user's skills into the skill menu.
    private void PopulateSkillMenu() {
        playerSkills = battleController.playerUnit.unit.skills;

        skillButtons = new List<SkillButtonController>();

        for (int i = 0; i < playerSkills.Count; i++) {
            // Get the skill and instantiate the button
            Skill skill = playerSkills[i];
            GameObject skillButton = Instantiate(skillButtonPrefab, content);

            // Setup the text on the button
            SkillButtonController buttonController = skillButton.GetComponent<SkillButtonController>();
            skillButtons.Add(buttonController);
            buttonController.dialogueBox = dialogueBox;
            buttonController.setData(skill);
            if (battleController.playerUnit.unit.cSP < skill.costSP)
                buttonController.RedNumber();

            // Register the listener for the button
            Button button = skillButton.GetComponent<Button>();
            button.interactable = !skill.requiresWeaponType ||
                (skill.requiresWeaponType && skill.requiredAtkType == battleController.playerUnit.unit.weapon.atkType);
            int j = i;
            button.onClick.AddListener(() => battleController.OnSkillButton(j));
        }
    }
}
