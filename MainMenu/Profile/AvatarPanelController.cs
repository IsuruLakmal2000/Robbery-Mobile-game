using UnityEngine;
using UnityEngine.UI;

public class AvatarPanelController : MonoBehaviour
{

    [SerializeField] private Transform gridContainer; // Parent container for the grid cells
    [SerializeField] private GameObject selectionIconPrefab; // Prefab for the selection icon
    [SerializeField] private Button closeBtn; // Prefab for the avatar
    private GameObject currentSelectionIcon; // Reference to the currently active selection icon

    void Start()
    {
        // Add listeners to all buttons in the grid
        foreach (Transform gridCell in gridContainer)
        {
            Button button = gridCell.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => OnAvatarSelected(gridCell));
            }
        }

        closeBtn.onClick.AddListener(() => Destroy(gameObject)); // Close the avatar panel when the close button is clicked
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
}