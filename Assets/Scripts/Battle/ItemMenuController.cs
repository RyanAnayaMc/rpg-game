using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemMenuController : MonoBehaviour
{
    [SerializeField]
    private Transform content; // The transform for the content field for the skills menu
    [SerializeField]
    private GameObject itemButtonPrefab; // The prefab for the item buttons
    [SerializeField]
    private BattleController battleController; // The battle controller for the battle
    [SerializeField]
    private TMP_Text dialogueBoxText; // The text on the dialogue box

    private List<SkillButtonController> itemButtons; // This controller is reused from skills

    public void SetItemMenu(List<InventoryItem> consumables) {
        // Create itemButtons array if it doesn't exist
        if (itemButtons == null)
            itemButtons = new List<SkillButtonController>();

        // Destroy all existing item buttons
        foreach (SkillButtonController buttonController in itemButtons)
            Destroy(buttonController.gameObject);

        // Populate the item list
        for (int i = 0; i < consumables.Count; i++) {
            // Get the item and instantiate the button
            InventoryItem item = consumables[i];
            GameObject itemButton = Instantiate(itemButtonPrefab, content);

            // Setup button text
            SkillButtonController buttonController = itemButton.GetComponent<SkillButtonController>();
            itemButtons.Add(buttonController);
            buttonController.dialogueText = dialogueBoxText;
            buttonController.setData(item);

            // Register the listener for the button
            Button button = itemButton.GetComponent<Button>();
            int j = i;
            button.onClick.AddListener(() => battleController.OnItemButton(j));
		}
    }
}
