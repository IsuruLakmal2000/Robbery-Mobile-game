[System.Serializable]
public class UserModel
{
    public string username;
    public string avatarId;
    public string frameId;    // The name of the icon for the achievement
    public int currentNetWorth;
    public int xpLevel;

    public UserModel(string username, int currentNetWorth, int level, string avatarId, string frameId)
    {
        this.username = username;
        this.currentNetWorth = currentNetWorth;
        this.xpLevel = level;
        this.avatarId = avatarId;
        this.frameId = frameId;
    }

}