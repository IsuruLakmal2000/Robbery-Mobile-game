[System.Serializable]
public class LeaderboardPlayerDetails
{
    public string avatarId;
    public string frameId;    // The name of the icon for the achievement
    public string username
    ;     // The name of the task
    public int currentNetworth;
    public int xpLevel;


    public LeaderboardPlayerDetails(string avatarId, string frameId, string username, int currentNetworth, int xpLevel)
    {
        this.avatarId = avatarId;
        this.frameId = frameId;
        this.username = username;
        this.currentNetworth = currentNetworth;
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
