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


    }


    public void ShowPopupPanel(string title, string content)
    {
        GameObject popupPanelInstance = Instantiate(popupPanel, canvas.transform);
        popupPanelInstance.transform.SetAsLastSibling();
        popupPanelInstance.GetComponent<PopupPanelController>().SetContent(title, content);
    }

    private void AvatarAndFrameUnlocking()
    {
        int currentLevel = PlayerPrefs.GetInt("current_level", 1);
        int totalMoney = PlayerPrefs.GetInt("total_money", 0);
        int consecutiveLogins = PlayerPrefs.GetInt("consecutive_logins", 0);
        bool isPremium = PlayerPrefs.GetInt("is_premium", 0) == 1;

        // Frame 1: Unlock by opening coffee shop business
        if (PlayerPrefs.GetInt("is_coffee_shop_opened", 0) == 1 && PlayerPrefs.GetInt("is_unlock_frame_frame1", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame1", 1);
            ShowRewardPanel("frame1");
        }

        // Frame 2: Unlock by collecting first 1M money
        if (totalMoney >= 1000000 && PlayerPrefs.GetInt("is_unlock_frame_frame2", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame2", 1);
            ShowRewardPanel("frame2");
        }

        // Frame 3: Unlock by logging in for 7 consecutive days
        if (consecutiveLogins >= 7 && PlayerPrefs.GetInt("is_unlock_frame_frame3", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame3", 1);
            ShowRewardPanel("frame3");
        }

        // Frame 4: Unlock by reaching XP level 5
        if (currentLevel >= 5 && PlayerPrefs.GetInt("is_unlock_frame_frame4", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame4", 1);
            ShowRewardPanel("frame4");
        }

        // Frame 5: Unlock by purchasing premium version or any in-app purchase
        if (isPremium && PlayerPrefs.GetInt("is_unlock_frame_frame5", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame5", 1);
            ShowRewardPanel("frame5");
        }

        // Frame 6: Unlock by reaching XP level 35
        if (currentLevel >= 35 && PlayerPrefs.GetInt("is_unlock_frame_frame6", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame6", 1);
            ShowRewardPanel("frame6");
        }

        // Frame 7: Unlock by reaching XP level 20
        if (currentLevel >= 20 && PlayerPrefs.GetInt("is_unlock_frame_frame7", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame7", 1);
            ShowRewardPanel("frame7");
        }

        // Frame 8: Unlock by collecting first 100k money
        if (totalMoney >= 100000 && PlayerPrefs.GetInt("is_unlock_frame_frame8", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame8", 1);
            ShowRewardPanel("frame8");
        }

        // Frame 9: Unlock by reaching XP level 50
        if (currentLevel >= 50 && PlayerPrefs.GetInt("is_unlock_frame_frame9", 0) == 0)
        {
            PlayerPrefs.SetInt("is_unlock_frame_frame9", 1);
            ShowRewardPanel("frame9");
        }
    }

    private void ShowRewardPanel(string frameId)
    {
        GameObject rewardPanelInstance = Instantiate(rewardPanelPrefab, canvas.transform);
        rewardPanelInstance.GetComponent<RewardPanelController>().SetAvatarFrameUnlockingDetails(frameId);
    }
}