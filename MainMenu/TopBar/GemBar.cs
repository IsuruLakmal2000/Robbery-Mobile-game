using UnityEngine;
using TMPro;


public class GemBar : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI gemBarCountTxt;

    private void Start()
    {
        gemBarCountTxt.text = PlayerPrefs.GetInt("total_gem", 0).ToString();
    }

    public void UpdateGemBar(int totalGem)
    {
        gemBarCountTxt.text = totalGem.ToString();
    }
    private void OnGUI()
    {
        gemBarCountTxt.text = PlayerPrefs.GetInt("total_gem", 0).ToString();
    }


}