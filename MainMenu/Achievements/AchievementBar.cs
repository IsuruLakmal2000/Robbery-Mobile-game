using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBar : MonoBehaviour
{

    [SerializeField] private GameObject TaskDetails;
    [SerializeField] private GameObject TaskCompleted;

    private Image iconImage;
    private Image rewardImg;
    private TextMeshProUGUI rewardCoundText;
    private TextMeshProUGUI taskNameText;
    private Slider barSlider;


    private


    void Awake()
    {
        TaskCompleted.SetActive(false);
        iconImage = TaskDetails.transform.Find("icon/IconImg").GetComponent<Image>();
        rewardCoundText = TaskDetails.transform.Find("Reward/rewardCountTxt").GetComponent<TextMeshProUGUI>();
        taskNameText = TaskDetails.transform.Find("TaskName").GetComponent<TextMeshProUGUI>();
        barSlider = TaskDetails.transform.Find("Slider").GetComponent<Slider>();
        rewardImg = TaskDetails.transform.Find("Reward").GetComponent<Image>();
        //  CheckUnlockAchievement();
    }

    public void SetTaskDetails(AchievementConfig achievementConfig)
    {
        string achivementID = achievementConfig.achievementID;
        Debug.Log("achivementID" + achivementID);
        //load achievemnt current level 
        int currentLevel = PlayerPrefs.GetInt("achievement_current_level" + achivementID, 1);
        Debug.Log("sub task -" + achievementConfig.subAchievements[currentLevel - 1].taskName + "current level" + currentLevel);
        // iconImage.sprite = Resources.Load<Sprite>("Achievements/" + achievementConfig.subAchievements[currentLevel - 1].iconName);
        taskNameText.text = achievementConfig.subAchievements[currentLevel - 1].taskName;
        rewardCoundText.text = achievementConfig.subAchievements[currentLevel - 1].rewardCount.ToString();
        // here use switch case to get specific data from palyer prefs ,because eahc achivements currnt slider value depend on the playeprefs values
        // barSlider.value = achievementConfig.subAchievements[currentLevel - 1].progress;
    }


    private void CheckTaskCompleted()
    {
        if (barSlider.value == 1)
        {
            TaskDetails.SetActive(false);
            TaskCompleted.SetActive(true);
        }
    }
}

