using UnityEngine;

public class MainMenuPanelController : MonoBehaviour
{

    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject createAccountPanel;
    [SerializeField] private GameObject rewardPanelPrefab;
    private Canvas canvas;

    public static MainMenuPanelController Instance;

    void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void Start()
    {

        canvas = FindFirstObjectByType<Canvas>();
        if (PlayerPrefs.GetInt("IsFirstTime", 1) == 1)
        {

            GameObject createAccInstance = Instantiate(createAccountPanel, canvas.transform);
            createAccInstance.transform.SetAsLastSibling();

        }

        AvatarAndFrameUnlocking();
    }


    public void ShowPopupPanel(string title, string content)
    {
        GameObject popupPanelInstance = Instantiate(popupPanel, canvas.transform);
        popupPanelInstance.transform.SetAsLastSibling();
        popupPanelInstance.GetComponent<PopupPanelController>().SetContent(title, content);
    }

    private void AvatarAndFrameUnlocking()
    {
        CheckConsecutiveLogins();
        int currentLevel = PlayerPrefs.GetInt("XP_Level", 1);
        int totalMoney = PlayerPrefs.GetInt("total_money", 0);
        int consecutiveLogins = PlayerPrefs.GetInt("consecutive_logins", 0);
        int totalGems = PlayerPrefs.GetInt("total_gems", 0);
        int policeCarsDestroyed = PlayerPrefs.GetInt("total_destroyed_police_car", 0);
        bool isCoffeShopOpen = PlayerPrefs.GetInt("coffeShop_level", 0) >= 1;
        bool isHotelBusinessUnlocked = PlayerPrefs.GetInt("hotel_level", 0) >= 1;
        bool isMillionaireClubUnlocked = PlayerPrefs.GetInt("is_unlock_millionare_club", 0) == 1;
        bool isBillionaireClubUnlocked = PlayerPrefs.GetInt("is_unlock_billionare_club", 0) == 1;
        bool isSecondaryGunUnlocked = PlayerPrefs.GetInt("is_unlock_V2", 0) == 1;

        // Frame Unlocking Conditions (Existing)
        if (PlayerPrefs.GetInt("hotel_level", 0) >= 1 && PlayerPrefs.GetInt("is_unlock_frame_frame1", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame1", 1);
            ShowRewardPanel("frame1");
        }
        if (totalMoney >= 1000000 && PlayerPrefs.GetInt("is_unlock_frame_frame2", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame2", 1);
            ShowRewardPanel("frame2");
        }
        if (consecutiveLogins >= 7 && PlayerPrefs.GetInt("is_unlock_frame_frame3", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame3", 1);
            ShowRewardPanel("frame3");
        }
        if (currentLevel >= 5 && PlayerPrefs.GetInt("is_unlock_frame_frame4", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame4", 1);
            ShowRewardPanel("frame4");
        }
        if (PlayerPrefs.GetInt("is_premium", 0) == 1 && PlayerPrefs.GetInt("is_unlock_frame_frame5", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame5", 1);
            ShowRewardPanel("frame5");
        }
        if (currentLevel >= 35 && PlayerPrefs.GetInt("is_unlock_frame_frame6", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame6", 1);
            ShowRewardPanel("frame6");
        }
        if (currentLevel >= 20 && PlayerPrefs.GetInt("is_unlock_frame_frame7", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame7", 1);
            ShowRewardPanel("frame7");
        }
        if (totalMoney >= 100000 && PlayerPrefs.GetInt("is_unlock_frame_frame8", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame8", 1);
            ShowRewardPanel("frame8");
        }
        if (currentLevel >= 50 && PlayerPrefs.GetInt("is_unlock_frame_frame9", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame9", 1);
            ShowRewardPanel("frame9");
        }

        // Avatar Unlocking Conditions (New)
        if (currentLevel >= 2 && PlayerPrefs.GetInt("is_unlock_avatar_avatar1", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar1", 1);
            ShowRewardPanel("avatar1");
        }
        if (totalGems >= 10 && PlayerPrefs.GetInt("is_unlock_avatar_avatar2", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar2", 1);
            ShowRewardPanel("avatar2");
        }
        if (policeCarsDestroyed >= 5 && PlayerPrefs.GetInt("is_unlock_avatar_avatar3", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar3", 1);
            ShowRewardPanel("avatar3");
        }
        if (isCoffeShopOpen && PlayerPrefs.GetInt("is_unlock_avatar_avatar4", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar4", 1);
            ShowRewardPanel("avatar4");
        }
        if (policeCarsDestroyed >= 50 && PlayerPrefs.GetInt("is_unlock_avatar_avatar5", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar5", 1);
            ShowRewardPanel("avatar5");
        }
        if (isMillionaireClubUnlocked && PlayerPrefs.GetInt("is_unlock_avatar_avatar6", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar6", 1);
            ShowRewardPanel("avatar6");
        }
        if (currentLevel >= 20 && PlayerPrefs.GetInt("is_unlock_avatar_avatar7", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar7", 1);
            ShowRewardPanel("avatar7");
        }
        if (isBillionaireClubUnlocked && PlayerPrefs.GetInt("is_unlock_avatar_avatar8", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar8", 1);
            ShowRewardPanel("avatar8");
        }
        if (policeCarsDestroyed >= 500 && PlayerPrefs.GetInt("is_unlock_avatar_avatar9", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar9", 1);
            ShowRewardPanel("avatar9");
        }
        if (isSecondaryGunUnlocked && PlayerPrefs.GetInt("is_unlock_avatar_avatar10", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar10", 1);
            ShowRewardPanel("avatar10");
        }
        if (currentLevel >= 100 && PlayerPrefs.GetInt("is_unlock_avatar_avatar11", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar11", 1);
            ShowRewardPanel("avatar11");
        }
        if (totalMoney >= 100000000 && PlayerPrefs.GetInt("is_unlock_avatar_avatar12", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_avatar_avatar12", 1);
            ShowRewardPanel("avatar12");
        }
    }
    void CheckConsecutiveLogins()
    {
        string lastLoginDate = PlayerPrefs.GetString("last_login_date", "");
        string currentDate = System.DateTime.Now.ToString("yyyy-MM-dd");

        if (lastLoginDate == currentDate)
        {
            // User already logged in today, do nothing
            return;

        }

        if (!string.IsNullOrEmpty(lastLoginDate))
        {
            System.DateTime lastDate = System.DateTime.Parse(lastLoginDate);
            System.DateTime today = System.DateTime.Now;

            if ((today - lastDate).Days == 1)
            {
                // Increment consecutive logins
                int consecutiveLogins = PlayerPrefs.GetInt("consecutive_logins", 0);
                PlayerPrefs.SetInt("consecutive_logins", consecutiveLogins + 1);
            }
            else
            {
                // Reset consecutive logins
                PlayerPrefs.SetInt("consecutive_logins", 1);
            }
        }
        else
        {
            // First login, set consecutive logins to 1
            PlayerPrefs.SetInt("consecutive_logins", 1);
        }

        // Update the last login date
        PlayerPrefs.SetString("last_login_date", currentDate);
        PlayerPrefs.Save();
    }

    private void ShowRewardPanel(string rewardName)
    {
        Debug.Log($"Unlocking {rewardName}");
        GameObject rewardPanelInstance = Instantiate(rewardPanelPrefab, canvas.transform);
        rewardPanelInstance.transform.SetAsLastSibling();
        rewardPanelInstance.GetComponent<RewardPanelController>().SetAvatarFrameUnlockingDetails(rewardName);
    }
}