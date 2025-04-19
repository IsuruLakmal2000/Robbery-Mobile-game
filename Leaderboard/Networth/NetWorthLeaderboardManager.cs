using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NetWorthLeaderoardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardBarPrefab; // Prefab for leaderboard bar

    [SerializeField] private TextMeshProUGUI networthLoading; // Panel to show leaderboard

    [SerializeField] private Button xpLeaderboardBtn;
    [SerializeField] private Button networthLeaderbardBtn;
    [SerializeField] private GameObject xpLeaderboardPanel; // Panel to show XP leaderboard
    [SerializeField] private GameObject networthLeaderboardPanel; // Panel to show net worth leaderboard

    void Start()
    {
        LoadNetworthLeaderboard();
        xpLeaderboardPanel.SetActive(false);
        networthLeaderboardPanel.SetActive(true);
        // Add listener to the button to load the XP leaderboard
        xpLeaderboardBtn.onClick.AddListener(LoadXpLeaderboard);
        networthLeaderbardBtn.onClick.AddListener(LoadNetworthLeaderboard);
    }
    public async void LoadNetworthLeaderboard()
    {
        networthLoading.text = "Loading...";
        // Clear existing leaderboard bars
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        // Get the top 50 users by XP level
        List<LeaderboardPlayerDetails> topUsers = await FirebaseController.instance.GetTopUsersByNetworth();
        foreach (var user in topUsers)
        {
            Debug.Log($"Player Name: {user.username}, Net Worth: {user.currentNetWorth}");
        }
        networthLoading.gameObject.SetActive(false);
        if (topUsers != null)
        {
            int index = 1;
            // Populate the leaderboard UI
            foreach (var user in topUsers)
            {
                GameObject bar = Instantiate(leaderboardBarPrefab, transform);
                Debug.Log("User: " + user.username + " Net Worth: " + user.currentNetWorth.ToString());
                bar.GetComponent<LeaderboardBarNetWorth>().SetLeaderboardDetails(user, index);
                index++;
            }
        }
        else
        {
            Debug.LogWarning("No users found for the XP leaderboard.");
        }

    }

    private void LoadXpLeaderboard()
    {
        networthLeaderboardPanel.SetActive(false);
        xpLeaderboardPanel.SetActive(true);
    }
}

