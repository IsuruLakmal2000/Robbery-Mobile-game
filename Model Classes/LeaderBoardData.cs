using System;

[System.Serializable]
public class LeaderBoardData
{
    public string userId;
    public string playerName;
    public int totalWorth;

    public int currentLevel;

    public string frameID;
    public string avatarID;

    public LeaderBoardData(string userId, string playerName, int currentLevel, string frameID, string avatarID)
    {
        this.userId = userId;
        this.playerName = playerName;

        this.currentLevel = currentLevel;

        this.frameID = frameID;
        this.avatarID = avatarID;
    }
}