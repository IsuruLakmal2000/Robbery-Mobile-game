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
        // PlayerPrefs.SetInt("is_unlock_frame_frame1", 1); // Example of setting the unlock status for avatar1
        // PlayerPrefs.SetInt("is_unlock_frame_frame2", 1); // Example of setting the unlock status for avatar2
        // PlayerPrefs.SetInt("is_unlock_frame_frame4", 1); // Example of setting the unlock status for avatar3
        // PlayerPrefs.SetInt("is_unlock_frame_frame9", 1); // Example of setting the unlock status for avatar4
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
            case "frame1":
                unlockMessage = "Unlock this frame by Opening Hotel  business.";
                break;
            case "frame2":
                unlockMessage = "Unlock  this frame  by collecting first 1M money.";
                break;
            case "frame3":
                unlockMessage = "Unlock  this frame  logging in for 7 consecutive days.";
                break;
            case "frame4":
                unlockMessage = "Unlock this frame by Reaching XP level 5.";
                break;
            case "frame5":
                unlockMessage = "Unlock  this frame  by Purchasing premium version or any in-app purchase.";
                break;
            case "frame6":
                unlockMessage = "Unlock this frame by Reaching XP level 35.";
                break;
            case "frame7":
                unlockMessage = "Unlock this frame by Reaching XP level 20.";
                break;
            case "frame8":
                unlockMessage = "Unlock  this frame  by collecting first 100k money.";
                break;
            case "frame9":
                unlockMessage = "Unlock this frame by Reaching XP level 50.";
                break;

            default:
                unlockMessage = "Unlock this avatar by completing the required task.";
                break;
        }



        return unlockMessage;
    }
}