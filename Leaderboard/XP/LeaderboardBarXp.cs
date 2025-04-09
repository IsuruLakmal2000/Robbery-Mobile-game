using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardBarXp : MonoBehaviour
{
    [SerializeField] private Image avatarImage;
    [SerializeField] private Image frameImage;

    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI playerNetworthText;
    [SerializeField] private TextMeshProUGUI playerRankText;
    [SerializeField] private TextMeshProUGUI playerXpLevelText;


    public void SetLeaderboardDetails(LeaderboardPlayerDetails leaderboardConfig, int rank)
    {
        // avatarImage.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + leaderboardConfig.avatarId);
        // frameImage.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + leaderboardConfig.frameId);
        playerNameText.text = leaderboardConfig.username;
        playerNetworthText.text = "Net Worth:" + FormatPrice(leaderboardConfig.currentNetWorth).ToString();
        playerRankText.text = rank.ToString();
        playerXpLevelText.text = leaderboardConfig.xpLevel.ToString();
    }

    private string FormatPrice(int price)
    {
        if (price >= 1000000) // 1M and above
            return (price / 1000000f).ToString("0.##") + "M";
        else if (price >= 1000) // 1K and above
            return (price / 1000f).ToString("0.##") + "K";
        else
            return price.ToString(); // If less than 1K, show as is
    }
}