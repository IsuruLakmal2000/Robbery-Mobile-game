using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private Image frame;
    [SerializeField] private Image avatar;
    [SerializeField] private GameObject profilePanelPrefab; // Prefab for the profile panel

    private Canvas canvas; // Prefab for the avatar panel

    public static ProfileManager instance; // Singleton instance

    private void Awake()
    {
        // Ensure only one instance of ProfileManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void Start()
    {
        // Load the profile data when the script starts
        canvas = FindAnyObjectByType<Canvas>();
        string currentAvatar = PlayerPrefs.GetString("CurrentAvatar", "defaultAvatar");
        string currentFrame = PlayerPrefs.GetString("CurrentFrame", "none");

        print($"Current Avatar: {currentAvatar}");
        print($"Current Frame: {currentFrame}");
        UpdateProfileUI(currentAvatar, currentFrame);
        gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject profilePanelInstance = Instantiate(profilePanelPrefab, canvas.transform);
            profilePanelInstance.transform.SetAsLastSibling(); // Bring the profile panel to the front
            // profilePanelInstance.
        });
    }

    public async void ChangeProfile(string name, string newAvatar, string newFrame)
    {
        string userId = PlayerPrefs.GetString("UserId"); // Retrieve the user ID from PlayerPrefs

        if (!string.IsNullOrEmpty(userId))
        {
            // Update the profile in Firebase

            await FirebaseController.instance.UpdateProfile(userId, newAvatar, newFrame);

            // Update the local UI

        }
        else
        {
            Debug.LogError("User ID is not set. Cannot update profile.");
        }
    }

    private void UpdateProfileUI(string newAvatar, string newFrame)
    {
        // Assuming you have a method to load sprites by name
        avatar.sprite = LoadSprite(newAvatar);
        if (newFrame == "none")
        {
            frame.gameObject.SetActive(false); // Hide the frame if it's "none"
            return;
        }
        else
        {
            frame.sprite = LoadSprite(newFrame);
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

        avatar.sprite = Resources.Load<Sprite>("Sprites/avatars/" + PlayerPrefs.GetString("CurrentAvatar", "defaul_avatar"));
    }

    private Sprite LoadSprite(string spriteName)
    {
        // Load the sprite from Resources or another location
        if (spriteName.StartsWith('a'))
        {
            return Resources.Load<Sprite>($"Sprites/avatars/{spriteName}");
        }
        else
        {
            return Resources.Load<Sprite>($"Sprites/frames/{spriteName}");
        }

    }
}