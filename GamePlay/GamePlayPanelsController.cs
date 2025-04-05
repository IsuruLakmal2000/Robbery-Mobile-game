using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelsController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
    [SerializeField] private GameObject watchAdPopupPanel;
    public static GamePlayPanelsController instance;
    [SerializeField] private Canvas canvas;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {


    }


    public void ShowWinPanel()
    {
        GameObject winPanelInstance = Instantiate(winPanel, canvas.transform);
        winPanelInstance.transform.SetAsLastSibling();

        int totalMoneyInthisLevel = GameRobbedMoney.instance.robbedMoneyCount;
        int totalGemsEarnedInthisLevel = GameRobbedMoney.instance.gameCollectedGemCount;
        WinPanelController.instance.SetRobbedMoney(totalMoneyInthisLevel, totalGemsEarnedInthisLevel);


    }

    public void ShowLoosePanel()
    {
        GameObject loosePanelInstance = Instantiate(loosePanel, canvas.transform);
        loosePanelInstance.transform.SetAsLastSibling();
        int fine = 1000;

        LoosePanelController.instance.SetFine(fine);
    }

    public void ShowWatchAddPopup(int price, string titile, string content, string type)
    {

        GameObject watchAddPopupInstance = Instantiate(watchAdPopupPanel, canvas.transform);
        watchAddPopupInstance.GetComponentInChildren<WatchAdsPopupPanelController>().SetData(content, titile, price, type);
    }
}