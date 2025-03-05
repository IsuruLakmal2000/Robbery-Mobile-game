[System.Serializable]
public class AchievementData
{
    public string iconName;    // The name of the icon for the achievement
    public string taskName;     // The name of the task
    public int rewardCount;     // The count of rewards associated with the achievement
    public float progress;       // The progress towards completing the achievement

    // Constructor for easy instantiation
    public AchievementData(string iconName, string taskName, int rewardCount, float progress)
    {
        //  this.iconName = iconName;
        this.taskName = taskName;
        //  this.rewardCount = rewardCount;
        //  this.progress = progress;
    }
}


[System.Serializable]
public class AchievementConfig
{
    public string achievementID;
    public AchievementData[] subAchievements;
}
