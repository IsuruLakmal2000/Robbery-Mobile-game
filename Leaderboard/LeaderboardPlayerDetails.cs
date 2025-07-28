using System.Collections.Generic;

// This file has been removed. No leaderboard functionality is used in this project anymore.

[System.Serializable]
public class LeaderboardPlayerDetails
{
    public string userId;
    public string avatarId;
    public string frameId;
    public string username;
    public int currentNetWorth;
    public int xpLevel;
    public int rank;

    public LeaderboardPlayerDetails(string userId, string avatarId, string frameId, string username, int currentNetWorth, int xpLevel, int rank)
    {
        this.userId = userId;
        this.avatarId = avatarId;
        this.frameId = frameId;
        this.username = username;
        this.currentNetWorth = currentNetWorth;
        this.xpLevel = xpLevel;
        this.rank = rank;
    }

    // Helper for hardcoded data
    public static List<LeaderboardPlayerDetails> GetHardcodedLeaderboard()
    {
        var users = new List<LeaderboardPlayerDetails>();
        // Add 9 hardcoded users
        users.Add(new LeaderboardPlayerDetails("u1", "avatar1", "frame1", "Alice", 1000000, 10, 1));
        users.Add(new LeaderboardPlayerDetails("u2", "avatar2", "frame2", "Bob", 900000, 9, 2));
        users.Add(new LeaderboardPlayerDetails("u3", "avatar3", "frame3", "Charlie", 800000, 8, 3));
        users.Add(new LeaderboardPlayerDetails("u4", "avatar4", "frame4", "David", 700000, 7, 4));
        users.Add(new LeaderboardPlayerDetails("u5", "avatar5", "frame5", "Eve", 600000, 6, 5));
        users.Add(new LeaderboardPlayerDetails("u6", "avatar6", "frame6", "Frank", 500000, 5, 6));
        users.Add(new LeaderboardPlayerDetails("u7", "avatar7", "frame7", "Grace", 400000, 4, 7));
        users.Add(new LeaderboardPlayerDetails("u8", "avatar8", "frame8", "Heidi", 300000, 3, 8));
        users.Add(new LeaderboardPlayerDetails("u9", "avatar9", "frame9", "Ivan", 200000, 2, 9));
        // Add 50 more realistic names
        string[] names = {
            "Sophia", "Jackson", "Olivia", "Liam", "Emma", "Noah", "Ava", "Lucas", "Mia", "Ethan",
            "Isabella", "Mason", "Charlotte", "Logan", "Amelia", "Elijah", "Harper", "James", "Abigail", "Benjamin",
            "Emily", "Jacob", "Ella", "Michael", "Elizabeth", "Daniel", "Camila", "Henry", "Luna", "Sebastian",
            "Sofia", "Jack", "Avery", "Alexander", "Scarlett", "William", "Victoria", "Matthew", "Madison", "Samuel",
            "Aria", "David", "Penelope", "Joseph", "Chloe", "Carter", "Layla", "Owen", "Riley", "Wyatt"
        };
        for (int i = 0; i < names.Length; i++)
        {
            int idx = i + 10;
            users.Add(new LeaderboardPlayerDetails($"u{idx}", $"avatar{(idx%12)+1}", $"frame{(idx%9)+1}", names[i], 200000-idx*2500, 1+(idx%100), idx));
        }
        return users;
    }
}
