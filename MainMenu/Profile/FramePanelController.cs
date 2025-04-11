using UnityEngine;
using UnityEngine.UI;

public class FramePanelController : MonoBehaviour
{
    [SerializeField] private Transform gridContainer;
    [SerializeField] private GameObject selectionIconPrefab;
    private GameObject currentSelectionIcon;
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject lockIcon;
    [SerializeField] private GameObject popupPrefab;
    void Start()
    {
        PlayerPrefs.SetInt("is_unlock_frame_frame1", 1); // Example of setting the unlock status for avatar1
        PlayerPrefs.SetInt("is_unlock_frame_frame2", 1); // Example of setting the unlock status for avatar2
        PlayerPrefs.SetInt("is_unlock_frame_frame4", 1); // Example of setting the unlock status for avatar3
        PlayerPrefs.SetInt("is_unlock_frame_frame9", 1); // Example of setting the unlock status for avatar4
        // Add listeners to all buttons in the grid
        foreach (Transform gridCell in gridContainer)
        {
            Button button = gridCell.GetComponent<Button>();
            // if (button != null)
            // {
            //     button.onClick.AddListener(() => OnFrameSelected(gridCell));
            // }
            if (PlayerPrefs.GetInt("is_unlock_frame_" + gridCell.gameObject.name) == 1)
            {
                button.interactable = true;
                if (button != null)
                {
                    button.onClick.AddListener(() => { OnFrameSelected(gridCell); }); // Add listener to the button
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

        closeBtn.onClick.AddListener(() => Destroy(gameObject)); // Close the frame panel when the close button is clicked
    }
    private void HowToUnlockPopup(Transform gridCell)
    {
        // Show a popup or message indicating how to unlock the avatar
        Debug.Log($"Frame {gridCell.gameObject.name} is locked. Show unlock instructions.");
        GameObject popupInstance = Instantiate(popupPrefab, transform);
        popupInstance.transform.SetAsLastSibling(); // Ensure the popup is on top of other elements

        popupInstance.GetComponent<PopupPanelController>().SetContent("Locked", GetUnlockMsg(gridCell)); // Assuming you have a method to set the frame name in the popup script
        // Implement your unlock instructions logic here
    }

    private void OnFrameSelected(Transform selectedCell)
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
        PlayerPrefs.SetString("CurrentFrame", selectedFrameName);
        PlayerPrefs.Save();

        Debug.Log($"Selected frame: {selectedFrameName}");
    }

    private string GetUnlockMsg(Transform gridCell)
    {
        // Show a popup or message indicating how to unlock the avatar
        Debug.Log($"Avatar {gridCell.gameObject.name} is locked. Show unlock instructions.");

        string unlockMessage = string.Empty;

        switch (gridCell.gameObject.name)
        {
            case "avatar1":
                unlockMessage = "Unlock avatar1 by completing level 1.";
                break;
            case "avatar2":
                unlockMessage = "Unlock avatar2 by collecting 100 coins.";
                break;
            case "avatar3":
                unlockMessage = "Unlock avatar3 by logging in for 7 consecutive days.";
                break;
            case "avatar4":
                unlockMessage = "Unlock avatar4 by purchasing the premium pack.";
                break;
            case "avatar5":
                unlockMessage = "Unlock avatar5 by sharing the app with a friend.";
                break;
            case "avatar6":
                unlockMessage = "Unlock avatar6 by completing the special event.";
                break;
            case "avatar7":
                unlockMessage = "Unlock avatar7 by reaching level 10.";
                break;
            case "avatar8":
                unlockMessage = "Unlock avatar8 by watching 5 ads.";
                break;
            case "avatar9":
                unlockMessage = "Unlock avatar9 by earning 500 points.";
                break;
            case "avatar10":
                unlockMessage = "Unlock avatar10 by completing the daily challenge.";
                break;
            case "avatar11":
                unlockMessage = "Unlock avatar11 by inviting 3 friends.";
                break;
            case "avatar12":
                unlockMessage = "Unlock avatar12 by achieving a high score of 1000.";
                break;
            default:
                unlockMessage = "Unlock this avatar by completing the required task.";
                break;
        }



        return unlockMessage;
    }
}