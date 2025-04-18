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
        if (leaderboardConfig.userId == PlayerPrefs.GetString("UserId"))
        {
            print("iside current user");
            gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = new Color(255, 205, 0, 255);
        }

        playerNameText.text = leaderboardConfig.username;
        playerNetworthText.text = FormatPrice(leaderboardConfig.currentNetWorth).ToString();
        playerRankText.text = rank.ToString();
        playerXpLevelText.text = "XP level:" + leaderboardConfig.xpLevel.ToString();
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