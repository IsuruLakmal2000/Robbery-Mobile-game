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
        // PlayerPrefs.SetInt("is_unlock_avatar_avatar1", 1); // Example of setting the unlock status for avatar1
        // PlayerPrefs.SetInt("is_unlock_avatar_avatar2", 1); // Example of setting the unlock status for avatar2
        // PlayerPrefs.SetInt("is_unlock_avatar_avatar4", 1); // Example of setting the unlock status for avatar3
        // PlayerPrefs.SetInt("is_unlock_avatar_avatar9", 1); // Example of setting the unlock status for avatar4
        // Add listeners to all buttons in the grid
        foreach (Transform gridCell in gridContainer)
        {
            Button button = gridCell.GetComponent<Button>();

            if (PlayerPrefs.GetInt("is_unlock_avatar_" + gridCell.gameObject.name) == 1)
            {
                button.interactable = true;
                if (button != null)
                {
                    button.onClick.AddListener(() =>
                    {
                        SoundManager.instance.PlayButtonClick();
                        OnAvatarSelected(gridCell);
                    }); // Add listener to the button
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
                button.onClick.AddListener(() =>
                {
                    SoundManager.instance.PlayButtonClick();
                    SoundManager.instance.PlayButtonClick();
                    HowToUnlockPopup(gridCell);
                }); // Add listener to the button
            }
        }

        closeBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            Destroy(gameObject);
        }); // Close the avatar panel when the close button is clicked
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
                avatarDescription = "Unlock this Avatar by reaching XP Level 2.";
                break;
            case "avatar2":
                avatarDescription = "Unlock this Avatar by Collecting 10 Gems.";
                break;
            case "avatar3":
                avatarDescription = " Unlock this Avatar by destroying 5 police cars.";
                break;
            case "avatar4":
                avatarDescription = "Unlock this Avatar by Unlocking Coffe business.";
                break;
            case "avatar5":
                avatarDescription = "Unlock this Avatar by destroying 50 police cars.";
                break;
            case "avatar6":
                avatarDescription = "Unlock this Avatar by Unlocking Millionaire Club.";
                break;
            case "avatar7":
                avatarDescription = " Unlock this Avatar by reaching XP Level 20";
                break;
            case "avatar8":
                avatarDescription = "Unlock this Avatar by Unlocking Billionare Club.";
                break;
            case "avatar9":
                avatarDescription = "Unlock this Avatar by Destroying 500 police cars.";
                break;
            case "avatar10":
                avatarDescription = "Unlock this Avatar by Unlocking secondary gun.";
                break;
            case "avatar11":
                avatarDescription = "Unlock this Avatar by reaching XP Level 100";
                break;
            case "avatar12":
                avatarDescription = "Unlock this Avatar by collecting 100M worth of money.";
                break;
            default:
                avatarDescription = "A unique avatar with its own story.";
                break;
        }

        return avatarDescription;
    }
}