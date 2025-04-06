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
        playerNetworthText.text = "Net Worth:" + leaderboardConfig.currentNetworth.ToString();
        playerRankText.text = rank.ToString();
        playerXpLevelText.text = leaderboardConfig.xpLevel.ToString();
    }


}