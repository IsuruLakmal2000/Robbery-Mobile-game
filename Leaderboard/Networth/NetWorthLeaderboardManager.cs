// This file has been removed. No leaderboard functionality is used in this project anymore.
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardBarPrefab;
    //public GameObject networthLoading;
    public GameObject networthLeaderboardPanel;
    public GameObject xpLeaderboardPanel;
     [SerializeField] private Button xpLeaderboardBtn;
    [SerializeField] private Button networthLeaderbardBtn;


    private void Start()
    {
        LoadNetworthLeaderboard();

        xpLeaderboardBtn.onClick.AddListener(LoadXpLeaderboard);
       // networthLeaderbardBtn.onClick.AddListener(LoadNetworthLeaderboard);
    }

    private void LoadXpLeaderboard()
    {
        xpLeaderboardPanel.SetActive(true);
        networthLeaderboardPanel.SetActive(false);
    }

    public void LoadNetworthLeaderboard()
    {
        var users = LeaderboardPlayerDetails.GetHardcodedLeaderboard();
        // Add current user
        string userId = PlayerPrefs.GetString("UserId", "current");
        string username = PlayerPrefs.GetString("UserName", "You");
        string avatarId = PlayerPrefs.GetString("CurrentAvatar", "avatar1");
        string frameId = PlayerPrefs.GetString("CurrentFrame", "frame1");
        int netWorth = PlayerPrefs.GetInt("total_networth", 0);
        int xpLevel = PlayerPrefs.GetInt("XP_Level", 1);
        var currentUser = new LeaderboardPlayerDetails(userId, avatarId, frameId, username, netWorth, xpLevel, 0);
        users.Add(currentUser);
        // Sort by net worth descending
        users = users.OrderByDescending(u => u.currentNetWorth).ToList();
        // Find current user rank
        int currentUserRank = users.FindIndex(u => u.userId == userId) + 1;
        currentUser.rank = currentUserRank;
        // Show top 20
        int rank = 1;
        foreach (Transform child in gameObject.transform) Destroy(child.gameObject);
        foreach (var user in users.Take(20))
        {
            GameObject bar = Instantiate(leaderboardBarPrefab, transform);
            bar.GetComponent<LeaderboardBarNetWorth>().SetLeaderboardDetails(user, rank);
            rank++;
        }
        // Ensure current user is displayed as the 21st element if not in the top 20
        if (currentUserRank > 20)
        {
            GameObject bar = Instantiate(leaderboardBarPrefab, transform);
            bar.GetComponent<LeaderboardBarNetWorth>().SetLeaderboardDetails(currentUser, currentUserRank);
        }
        // Show current user details and rank (customize as needed)
        Debug.Log($"Current User: {username}, Rank: {currentUserRank}");
      ///  networthLoading.gameObject.SetActive(false);
    }
}

