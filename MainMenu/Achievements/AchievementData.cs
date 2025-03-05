[System.Serializable]
public class AchievementData
{
    public string iconName;    // The name of the icon for the achievement
    public string taskName;     // The name of the task
    public int rewardCount;
    public string rewardType;

    public int barMaxValue;
    // The progress towards completing the achievement

    // Constructor for easy instantiation
    public AchievementData(string iconName, string taskName, int rewardCount, int barMaxValue, string rewardType)
    {
        this.iconName = iconName;
        this.taskName = taskName;
        this.rewardCount = rewardCount;
        this.barMaxValue = barMaxValue;
        this.rewardType = rewardType;

    }
}


[System.Serializable]
public class AchievementConfig
{
    public string achievementID;
    public string iconID;
    public AchievementData[] subAchievements;
}
