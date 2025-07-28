using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XpLeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardBarPrefab; // Prefab for leaderboard bar
    [SerializeField] private Button xpLeaderboardBtn;
    [SerializeField] private Button networthLeaderbardBtn;
    [SerializeField] private GameObject xpLeaderboardPanel; // Panel to show XP leaderboard
    [SerializeField] private GameObject networthLeaderboardPanel; // Panel to show net worth leaderboard
    //[SerializeField] private TextMeshProUGUI xpLoading; // Panel to show leaderboard

    void Start()
    {
        LoadXpLeaderboard();

        // Add listener to the button to load the XP leaderboard
        xpLeaderboardBtn.onClick.AddListener(LoadXpLeaderboard);
        networthLeaderbardBtn.onClick.AddListener(LoadNetworthLeaderboard);
    }
    public void LoadXpLeaderboard()
    {
       // xpLoading.text = "Loading...";
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        var users = LeaderboardPlayerDetails.GetHardcodedLeaderboard();
        string userId = PlayerPrefs.GetString("UserId", "current");
        string username = PlayerPrefs.GetString("UserName", "You");
        string avatarId = PlayerPrefs.GetString("CurrentAvatar", "avatar1");
        string frameId = PlayerPrefs.GetString("CurrentFrame", "frame1");
        int netWorth = PlayerPrefs.GetInt("total_networth", 0);
        int xpLevel = PlayerPrefs.GetInt("XP_Level", 1);
        var currentUser = new LeaderboardPlayerDetails(userId, avatarId, frameId, username, netWorth, xpLevel, 0);
        users.Add(currentUser);
        users = users.OrderByDescending(u => u.xpLevel).ToList();
        int currentUserRank = users.FindIndex(u => u.userId == userId) + 1;
        currentUser.rank = currentUserRank;
        int rank = 1;
        foreach (var user in users.Take(20))
        {
            GameObject bar = Instantiate(leaderboardBarPrefab, transform);
            bar.GetComponent<LeaderboardBarXp>().SetLeaderboardDetails(user, rank);
            rank++;
        }

        // Ensure current user is displayed as the 21st element if not in the top 20
        if (currentUserRank > 20)
        {
            GameObject bar = Instantiate(leaderboardBarPrefab, transform);
            bar.GetComponent<LeaderboardBarXp>().SetLeaderboardDetails(currentUser, currentUserRank);
        }

        Debug.Log($"Current User: {username}, Rank: {currentUserRank}");
       // xpLoading.gameObject.SetActive(false);
    }

    private void LoadNetworthLeaderboard()
    {
        xpLeaderboardPanel.SetActive(false);
        networthLeaderboardPanel.SetActive(true);
    }
}

