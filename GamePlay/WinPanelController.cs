using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinPanelController : MonoBehaviour
{

    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI robbedMoneyTxt;
    public static WinPanelController instance;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        nextBtn.onClick.AddListener(OnNextBtnClick);

    }

    private void OnNextBtnClick()
    {
        Debug.Log("on next clicked ");
        SceneManager.LoadScene("Menu");
        // Load next level
    }

    public void SetRobbedMoney(int money)
    {
        robbedMoneyTxt.text = money.ToString();
    }
}