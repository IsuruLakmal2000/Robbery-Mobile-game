using UnityEngine;
using TMPro;


public class TotalNetWorthBar : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI totalNetWorthTxt;

    private void Start()
    {
        totalNetWorthTxt.text = PlayerPrefs.GetInt("total_money", 0).ToString();
    }

    public void UpdateTotalNetWorthBar(int totalMoney)
    {
        totalNetWorthTxt.text = totalMoney.ToString();
    }
    private void OnGUI()
    {
        totalNetWorthTxt.text = PlayerPrefs.GetInt("total_money", 0).ToString();
    }
}