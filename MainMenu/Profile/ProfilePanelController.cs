using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanelController : MonoBehaviour
{
    [SerializeField] private Button changeAvatarBtn;
    [SerializeField] private Button changeFrameBtn;

    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject avatarPanel;
    [SerializeField] private GameObject framePanel;
    [SerializeField] private TMP_InputField nameField; // Prefab for the profile panel
    [SerializeField] private Image frame;
    [SerializeField] private Image avatar;
    private Canvas canvas; // Prefab for the avatar panel

    void Start()
    {
        canvas = FindAnyObjectByType<Canvas>();
        changeAvatarBtn.onClick.AddListener(OnChangeAvatarBtnClicked);
        changeFrameBtn.onClick.AddListener(OnChangeFrameBtnClicked);
        closeBtn.onClick.AddListener(OnCloseBtnClicked);
        nameField.onEndEdit.AddListener(OnNameFieldEdited);

        // Load the current name from PlayerPrefs
        if (PlayerPrefs.HasKey("UserName"))
        {
            nameField.text = PlayerPrefs.GetString("UserName");
        }
    }


    private void OnGUI()
    {
        string framename = PlayerPrefs.GetString("CurrentFrame", "none");
        if (framename == "none")
        {
            frame.gameObject.SetActive(false); // Hide the frame if it's "none"
            return;
        }
        else
        {
            frame.sprite = Resources.Load<Sprite>("Sprites/frames/" + framename);
        }

        avatar.sprite = Resources.Load<Sprite>("Sprites/avatars/" + PlayerPrefs.GetString("CurrentAvatar", "default_avatar"));
    }

    private void OnNameFieldEdited(string newName)
    {
        if (!string.IsNullOrEmpty(newName))
        {
            // Save the updated name to PlayerPrefs
            PlayerPrefs.SetString("UserName", newName);
            PlayerPrefs.Save(); // Ensure the changes are saved immediately
            Debug.Log($"User name updated to: {newName}");
        }
        else
        {
            Debug.LogWarning("Name field is empty. No changes saved.");
        }
    }
    private void OnChangeAvatarBtnClicked()
    {
        // Logic to change avatar
        GameObject avatarPanelInstance = Instantiate(avatarPanel, canvas.transform);
        avatarPanelInstance.transform.SetAsLastSibling(); // Bring the avatar panel to the front

        Debug.Log("Change Avatar button clicked.");
    }

    private void OnChangeFrameBtnClicked()
    {
        // Logic to change frame
        GameObject framePanelInstance = Instantiate(framePanel, canvas.transform);
        framePanelInstance.transform.SetAsLastSibling(); // Bring the frame panel to the front
        Debug.Log("Change Frame button clicked.");
    }

    private void OnCloseBtnClicked()
    {
        ProfileManager.instance.ChangeProfile(nameField.text, PlayerPrefs.GetString("CurrentAvatar", "default_avatar"), PlayerPrefs.GetString("CurrentFrame", "none"));
        Destroy(gameObject); // Close the profile panel by destroying it
    }
}