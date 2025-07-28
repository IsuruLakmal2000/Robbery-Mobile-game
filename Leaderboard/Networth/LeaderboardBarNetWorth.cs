using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardBarNetWorth : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    [SerializeField] private Image frameImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerNetworthText;
    [SerializeField] private TextMeshProUGUI playerRankText;
    [SerializeField] private TextMeshProUGUI playerXpLevelText;

    public void SetLeaderboardDetails(LeaderboardPlayerDetails leaderboardConfig, int rank)
    {
        avatarImage.sprite = Resources.Load<Sprite>("Sprites/avatars/" + leaderboardConfig.avatarId);
        if (leaderboardConfig.frameId != "none")
        {
            frameImage.sprite = Resources.Load<Sprite>("Sprites/frames/" + leaderboardConfig.frameId);
        }
        else
        {
            frameImage.gameObject.SetActive(false);
        }
        playerNameText.text = leaderboardConfig.username;
        playerNetworthText.text = FormatPrice(leaderboardConfig.currentNetWorth);
        playerRankText.text = rank.ToString();
        playerXpLevelText.text = "XP level: " + leaderboardConfig.xpLevel.ToString();

        // Highlight current user
        if (leaderboardConfig.userId == PlayerPrefs.GetString("UserId", "current"))
        {
            // Set background color for current user (customize as needed)
            GetComponent<Image>().color = new Color(1f, 0.92f, 0.23f, 1f); // Gold/yellow
        }
        else
        {
            GetComponent<Image>().color = Color.white;
        }
    }

    private string FormatPrice(int price)
    {
        if (price >= 1000000)
            return (price / 1000000f).ToString("0.##") + "M";
        else if (price >= 1000)
            return (price / 1000f).ToString("0.##") + "K";
        else
            return price.ToString();
    }
}