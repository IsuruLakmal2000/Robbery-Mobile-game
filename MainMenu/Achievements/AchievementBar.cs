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

        int currentLevel = PlayerPrefs.GetInt("achievement_current_level" + achievementID, 1);
        Debug.Log("current level is " + currentLevel);
        Debug.Log("currentlevel task - " + achievementConfig.subAchievements[currentLevel - 1].taskName);
        Debug.Log("previous task- " + achievementConfig.subAchievements[currentLevel].taskName);
        rewardImg.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + achievementConfig.subAchievements[currentLevel - 1].rewardType);
        iconImage.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + achievementConfig.iconID);
        taskNameText.text = achievementConfig.subAchievements[currentLevel - 1].taskName;
        rewardCoundText.text = achievementConfig.subAchievements[currentLevel - 1].rewardCount.ToString();
        barSlider.maxValue = achievementConfig.subAchievements[currentLevel - 1].barMaxValue;
        barSlider.value = GetSliderValue(achievementID);
        barSlider.gameObject.transform.Find("text").GetComponent<TextMeshProUGUI>().text = barSlider.value + "/" + barSlider.maxValue;
        CheckTaskCompleted(currentLevel, achievementConfig);

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
            TaskCompleted.GetComponent<Button>().onClick.AddListener(() =>
            {
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
        Debug.Log("Claimed reward for " + achievementConfigInThisBar.subAchievements[currentLevel - 1].taskName);
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

            default:
                return 0;
        }
    }

}

