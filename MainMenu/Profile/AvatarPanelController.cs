using UnityEngine;
using UnityEngine.UI;

public class AvatarPanelController : MonoBehaviour
{

    [SerializeField] private Transform gridContainer; // Parent container for the grid cells
    [SerializeField] private GameObject selectionIconPrefab; // Prefab for the selection icon
    [SerializeField] private GameObject lockIcon; // Prefab for the selection icon
    [SerializeField] private Button closeBtn; // Prefab for the avatar
    private GameObject currentSelectionIcon; // Reference to the currently active selection icon
    [SerializeField] private GameObject popupPrefab;
    void Start()
    {
        PlayerPrefs.SetInt("is_unlock_avatar_avatar1", 1); // Example of setting the unlock status for avatar1
        PlayerPrefs.SetInt("is_unlock_avatar_avatar2", 1); // Example of setting the unlock status for avatar2
        PlayerPrefs.SetInt("is_unlock_avatar_avatar4", 1); // Example of setting the unlock status for avatar3
        PlayerPrefs.SetInt("is_unlock_avatar_avatar9", 1); // Example of setting the unlock status for avatar4
        // Add listeners to all buttons in the grid
        foreach (Transform gridCell in gridContainer)
        {
            Button button = gridCell.GetComponent<Button>();

            if (PlayerPrefs.GetInt("is_unlock_avatar_" + gridCell.gameObject.name) == 1)
            {
                button.interactable = true;
                if (button != null)
                {
                    button.onClick.AddListener(() => { OnAvatarSelected(gridCell); }); // Add listener to the button
                }
            }
            else
            {
                button.interactable = true; // Disable the button if the avatar is not unlocked
                button.GetComponent<Image>().color = new Color(1, 1, 1, 0.9f); // Make the button semi-transparent
                GameObject lockIconInstance = Instantiate(lockIcon, gridCell);

                // Position the lock icon at the top-right corner with padding
                RectTransform lockIconRect = lockIconInstance.GetComponent<RectTransform>();
                lockIconRect.anchorMin = new Vector2(1, 1);
                lockIconRect.anchorMax = new Vector2(1, 1);
                lockIconRect.pivot = new Vector2(1, 1);
                lockIconRect.anchoredPosition = new Vector2(-10, -10); // Adjust padding as needed

                lockIconInstance.transform.SetAsLastSibling();
                button.onClick.AddListener(() => { HowToUnlockPopup(gridCell); }); // Add listener to the button
            }
        }

        closeBtn.onClick.AddListener(() => Destroy(gameObject)); // Close the avatar panel when the close button is clicked
    }
    private void HowToUnlockPopup(Transform gridCell)
    {
        Debug.Log($"Avatar {gridCell.gameObject.name} is locked. Show unlock instructions.");
        GameObject popupInstance = Instantiate(popupPrefab, transform);
        popupInstance.transform.SetAsLastSibling(); // Ensure the popup is on top of other elements

        popupInstance.GetComponent<PopupPanelController>().SetContent("Locked", GetAvatarDescription(gridCell)); // Assuming you have a method to set the frame name in the popup script
        // Implement your unlock instructions logic here
    }
    private void OnAvatarSelected(Transform selectedCell)
    {
        // Remove the previous selection icon if it exists
        if (currentSelectionIcon != null)
        {
            Destroy(currentSelectionIcon);
        }

        // Add a new selection icon to the selected cell
        currentSelectionIcon = Instantiate(selectionIconPrefab, selectedCell);
        currentSelectionIcon.transform.SetAsLastSibling(); // Ensure the icon is on top of other elements

        // Save the selected frame's name in PlayerPrefs
        string selectedFrameName = selectedCell.gameObject.name;
        PlayerPrefs.SetString("CurrentAvatar", selectedFrameName);
        PlayerPrefs.Save();

        Debug.Log($"Selected avatar: {selectedFrameName}");
    }

    private string GetAvatarDescription(Transform gridCell)
    {
        string avatarDescription = string.Empty;

        switch (gridCell.gameObject.name)
        {
            case "avatar1":
                avatarDescription = "Avatar1: A brave warrior from the ancient lands.";
                break;
            case "avatar2":
                avatarDescription = "Avatar2: A cunning rogue with unmatched agility.";
                break;
            case "avatar3":
                avatarDescription = "Avatar3: A wise mage with powerful spells.";
                break;
            case "avatar4":
                avatarDescription = "Avatar4: A noble knight with a strong sense of justice.";
                break;
            case "avatar5":
                avatarDescription = "Avatar5: A mysterious assassin from the shadows.";
                break;
            case "avatar6":
                avatarDescription = "Avatar6: A cheerful bard spreading joy and music.";
                break;
            case "avatar7":
                avatarDescription = "Avatar7: A fierce barbarian with unmatched strength.";
                break;
            case "avatar8":
                avatarDescription = "Avatar8: A skilled archer with deadly precision.";
                break;
            case "avatar9":
                avatarDescription = "Avatar9: A stealthy ninja with incredible speed.";
                break;
            case "avatar10":
                avatarDescription = "Avatar10: A fearless adventurer seeking glory.";
                break;
            case "avatar11":
                avatarDescription = "Avatar11: A powerful sorcerer mastering the elements.";
                break;
            case "avatar12":
                avatarDescription = "Avatar12: A legendary hero of ancient myths.";
                break;
            default:
                avatarDescription = "A unique avatar with its own story.";
                break;
        }

        return avatarDescription;
    }
}