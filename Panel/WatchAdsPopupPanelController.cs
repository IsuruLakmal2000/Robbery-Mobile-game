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

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
        okBtn.onClick.AddListener(() =>
        {
            Destroy(gameObject);

        });

    }

    public void SetData(string content, string title, double price, string type)
    {
        contentTxt.text = content;
        titleTxt.text = title;

        switch (type)
        {
            case "money":
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