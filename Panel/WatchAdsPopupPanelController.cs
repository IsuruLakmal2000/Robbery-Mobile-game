using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WatchAdsPopupPanelController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI contentTxt;
    [SerializeField] private TextMeshProUGUI titleTxt;
    [SerializeField] private Button okBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private GameObject moneyIcon;
    private int RewardAmount = 0;
    private string RewardType = "";

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
        });
        okBtn.onClick.AddListener(() =>
        {
            AdManager.instance.ShowRewardedInterstitialAd(RewardType, RewardAmount);
            Destroy(gameObject);

        });

    }

    public void SetData(string content, string title, int price, string type)
    {
        contentTxt.text = content;
        titleTxt.text = title;
        RewardAmount = price;
        RewardType = type;

        switch (type)
        {
            case "watch_ad_money_gameplay":
                GameObject MoneyInstance = Instantiate(moneyIcon, transform);
                MoneyInstance.transform.Find("rewardCount").GetComponent<TextMeshProUGUI>().text = FormatPrice(price).ToString();
                break;

        }
    }

    private string FormatPrice(double price)
    {
        if (price >= 1000000) // 1M and above
            return (price / 1000000f).ToString("0.###") + "M";
        else if (price >= 1000) // 1K and above
            return (price / 1000f).ToString("0.###") + "K";
        else
            return price.ToString(); // If less than 1K, show as is
    }

}