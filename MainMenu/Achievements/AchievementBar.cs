using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBar : MonoBehaviour
{

    [SerializeField] private GameObject TaskDetails;
    [SerializeField] private GameObject TaskCompleted;

    private Image iconImage;
    private Image rewardImg;
    [SerializeField] private TextMeshProUGUI rewardCoundText;
    [SerializeField] private TextMeshProUGUI taskNameText;
    private Slider barSlider;
    [SerializeField] private string achievementID;
    [SerializeField] private AchievementConfig achievementConfigInThisBar;
    [SerializeField] private GameObject rewardPanelPrefab;


    private


    void Awake()
    {
        TaskCompleted.SetActive(false);
        TaskDetails.SetActive(true);
        iconImage = TaskDetails.transform.Find("icon/IconImg").GetComponent<Image>();
        rewardCoundText = TaskDetails.transform.Find("Reward/rewardCountTxt").GetComponent<TextMeshProUGUI>();
        taskNameText = TaskDetails.transform.Find("TaskName").GetComponent<TextMeshProUGUI>();
        barSlider = TaskDetails.transform.Find("Slider").GetComponent<Slider>();
        rewardImg = TaskDetails.transform.Find("Reward").GetComponent<Image>();
        //  CheckUnlockAchievement();
    }

    public void SetTaskDetails(AchievementConfig achievementConfig)
    {
        TaskDetails.SetActive(true);
        TaskCompleted.SetActive(false);
        achievementConfigInThisBar = achievementConfig;
        achievementID = achievementConfig.achievementID;


        iconImage = TaskDetails.transform.Find("icon/IconImg").GetComponent<Image>();
        rewardCoundText = TaskDetails.transform.Find("Reward/rewardCountTxt").GetComponent<TextMeshProUGUI>();
        taskNameText = TaskDetails.transform.Find("TaskName").GetComponent<TextMeshProUGUI>();
        barSlider = TaskDetails.transform.Find("Slider").GetComponent<Slider>();
        rewardImg = TaskDetails.transform.Find("Reward").GetComponent<Image>();

        int currentLevel = PlayerPrefs.GetInt("achievement_current_level" + achievementID, 1);
        if (currentLevel > achievementConfig.subAchievements.Length)
        {
            Destroy(gameObject);
        }
        else
        {
            rewardImg.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + achievementConfig.subAchievements[currentLevel - 1].rewardType);
            iconImage.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + achievementConfig.iconID);
            taskNameText.text = achievementConfig.subAchievements[currentLevel - 1].taskName;
            rewardCoundText.text = achievementConfig.subAchievements[currentLevel - 1].rewardCount.ToString();
            barSlider.maxValue = achievementConfig.subAchievements[currentLevel - 1].barMaxValue;
            barSlider.value = GetSliderValue(achievementID);
            barSlider.gameObject.transform.Find("text").GetComponent<TextMeshProUGUI>().text = barSlider.value + "/" + barSlider.maxValue;
            CheckTaskCompleted(currentLevel, achievementConfig);
        }


    }


    private void CheckTaskCompleted(int currentLevel, AchievementConfig achievementConfig)
    {

        if (barSlider.value >= achievementConfigInThisBar.subAchievements[currentLevel - 1].barMaxValue)
        {

            TaskDetails.SetActive(false);
            TaskCompleted.SetActive(true);
            taskNameText = TaskCompleted.transform.Find("TaskName").GetComponent<TextMeshProUGUI>();
            taskNameText.text = achievementConfigInThisBar.subAchievements[currentLevel - 1].taskName;
            rewardCoundText = TaskCompleted.transform.Find("Reward/count").GetComponent<TextMeshProUGUI>();
            rewardCoundText.text = achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardCount.ToString();
            rewardImg = TaskCompleted.transform.Find("Reward").GetComponent<Image>();
            rewardImg.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardType);
            iconImage = TaskCompleted.transform.Find("IconImg").GetComponent<Image>();
            iconImage.sprite = Resources.Load<Sprite>("Sprites/Task/icons/check");
            Button claimButton = TaskCompleted.GetComponent<Button>();
            claimButton.onClick.RemoveAllListeners();
            claimButton.onClick.AddListener(() =>
            {
                if (achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardType == "Money")
                {
                    SoundManager.instance.PlayMoneyIncreaseSound();
                }
                else
                {
                    SoundManager.instance.PlayAlertInfoSound();
                }
                GameObject rewardPanelInstance = Instantiate(rewardPanelPrefab, transform.parent.parent.parent.parent.parent.parent);
                rewardPanelInstance.GetComponent<RewardPanelController>().SetRewardDetails(achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardType, achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardCount);
                SaveRewards(achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardType, achievementConfigInThisBar.subAchievements[currentLevel - 1].rewardCount);
                ClaimReward(currentLevel);
            });

        }
        else
        {
            Debug.Log("task not complete");
            return;
        }
    }

    private void ClaimReward(int currentLevel)
    {
        PlayerPrefs.SetInt("achievement_current_level" + achievementID, currentLevel + 1);
        PlayerPrefs.Save();
        TaskDetails.SetActive(true);
        TaskCompleted.SetActive(false);
        SetTaskDetails(achievementConfigInThisBar);
    }


    public float GetSliderValue(string achievementID)
    {
        switch (achievementID)
        {
            case "ach_1":
                int currentEarnedMoney = PlayerPrefs.GetInt("total_money", 0);
                // float currentEarnedMoneyFloat = (float)currentEarnedMoney;
                return currentEarnedMoney;
            case "ach_2":
                int xpLevel = PlayerPrefs.GetInt("XP_Level", 0);
                //float xpLevelFloat = (float)xpLevel;
                return xpLevel;
            case "ach_3":
                int totalDestroyedPoliceVehicles = PlayerPrefs.GetInt("total_destroyed_police_vehicles", 0);
                return totalDestroyedPoliceVehicles;
            case "ach_4":
                int isUnlock = PlayerPrefs.GetInt("coffeShop_level", 0);
                return isUnlock;
            case "ach_5":
                int isUnlockHotel = PlayerPrefs.GetInt("hotel_level", 0);
                return isUnlockHotel;
            case "ach_6":
                int isUnlockMillionare = PlayerPrefs.GetInt("is_unlock_millionare_club", 0);
                return isUnlockMillionare;
            case "ach_7":
                int isUnlockBillionare = PlayerPrefs.GetInt("is_unlock_billionare_club", 0);
                return isUnlockBillionare;
            case "ach_8":
                int coffeshopLevel = PlayerPrefs.GetInt("coffeShop_level", 0);
                return coffeshopLevel;
            case "ach_9":
                int hotelLevel = PlayerPrefs.GetInt("hotel_level", 0);
                return hotelLevel;

            default:
                return 0;
        }
    }

    private void SaveRewards(string rewardType, int rewardCount)
    {
        switch (rewardType)
        {
            case "Money":
                int currentMoney = PlayerPrefs.GetInt("total_money", 0);
                currentMoney += rewardCount;
                PlayerPrefs.SetInt("total_money", currentMoney);
                PlayerPrefs.Save();
                break;

            case "Exp":
                //int currentXP = PlayerPrefs.GetInt("XP_Level", 0);
                XPSystem.Instance.AddXP(rewardCount);
                // currentXP += rewardCount;
                // PlayerPrefs.SetInt("XP_Level", currentXP);
                // PlayerPrefs.Save();
                break;
            case "Gem":
                // int currentGem = PlayerPrefs.GetInt("total_gem", 0);
                // currentGem += rewardCount;
                // PlayerPrefs.SetInt("total_gem", currentGem);
                // PlayerPrefs.Save();
                break;

            default:
                Debug.Log("Reward type not found");
                break;
        }
    }

}

