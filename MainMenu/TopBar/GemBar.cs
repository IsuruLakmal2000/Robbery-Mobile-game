using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class GemBar : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI gemBarCountTxt;
    [SerializeField] private Button moreBtn; // Reference to the loading text UI element

    private void Start()
    {
        gemBarCountTxt.text = PlayerPrefs.GetInt("total_gem", 0).ToString();
        moreBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            BottomBarController.instance.OnShopBtnClick();
        });
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