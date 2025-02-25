using UnityEngine;
using TMPro;


public class TotalNetWorthBar : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI totalNetWorthTxt;

    private void Start()
    {
        totalNetWorthTxt.text = FormatPrice(PlayerPrefs.GetInt("total_money", 0));
    }

    public void UpdateTotalNetWorthBar(int totalMoney)
    {
        totalNetWorthTxt.text = totalMoney.ToString();
    }
    private void OnGUI()
    {
        totalNetWorthTxt.text = FormatPrice(PlayerPrefs.GetInt("total_money", 0));
    }

    private string FormatPrice(int price)
    {
        if (price >= 1000000) // 1M and above
            return (price / 1000000f).ToString("0.#") + "M";
        else if (price >= 10000) // 1K and above
            return (price / 1000f).ToString("0.#") + "K";
        else
            return price.ToString(); // If less than 1K, show as is
    }
}