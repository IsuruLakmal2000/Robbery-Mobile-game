using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpLeaderboardManager : MonoBehaviour
{
    [SerializeField] private GameObject leaderboardBarPrefab; // Prefab for leaderboard bar
    [SerializeField] private Button xpLeaderboardBtn;
    [SerializeField] private Button networthLeaderbardBtn;
    [SerializeField] private GameObject xpLeaderboardPanel; // Panel to show XP leaderboard
    [SerializeField] private GameObject networthLeaderboardPanel; // Panel to show net worth leaderboard

    void Start()
    {
        LoadXpLeaderboard();

        // Add listener to the button to load the XP leaderboard
        xpLeaderboardBtn.onClick.AddListener(LoadXpLeaderboard);
        networthLeaderbardBtn.onClick.AddListener(LoadNetworthLeaderboard);
    }
    public async void LoadXpLeaderboard()
    {
        // Clear existing leaderboard bars
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        // Get the top 50 users by XP level
        List<LeaderboardPlayerDetails> topUsers = await FirebaseController.instance.GetTopUsersByXPLevel();
        foreach (var user in topUsers)
        {
            Debug.Log($"Player Name- in xo: {user.username}, Net Worth: {user.currentNetworth}");
        }
        if (topUsers != null)
        {
            int index = 1;
            // Populate the leaderboard UI
            foreach (var user in topUsers)
            {

                GameObject bar = Instantiate(leaderboardBarPrefab, transform);
                Debug.Log("User: " + user.username + " Net Worth: " + user.currentNetworth.ToString() + " XP Level: " + user.xpLevel.ToString());
                bar.GetComponent<LeaderboardBarXp>().SetLeaderboardDetails(user, index);
                index++;

            }
        }
        else
        {
            Debug.LogWarning("No users found for the XP leaderboard.");
        }
    }

    private void LoadNetworthLeaderboard()
    {
        xpLeaderboardPanel.SetActive(false);
        networthLeaderboardPanel.SetActive(true);
    }
}

