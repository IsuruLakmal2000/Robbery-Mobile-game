[System.Serializable]
public class LeaderboardPlayerDetails
{
    public string avatarId;
    public string frameId;    // The name of the icon for the achievement
    public string username
    ;     // The name of the task
    public int currentNetWorth;
    public int xpLevel;


    public LeaderboardPlayerDetails(string avatarId, string frameId, string username, int currentNetWorth, int xpLevel)
    {
        this.avatarId = avatarId;
        this.frameId = frameId;
        this.username = username;
        this.currentNetWorth = currentNetWorth;
        this.xpLevel = xpLevel;

    }
}


// [System.Serializable]
// public class AchievementConfig
// {
//     public string achievementID;
//     public string iconID;
//     public AchievementData[] subAchievements;
// }
