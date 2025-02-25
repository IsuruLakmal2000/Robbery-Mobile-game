using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelsController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject loosePanel;
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
        WinPanelController.instance.SetRobbedMoney(totalMoneyInthisLevel);


    }

    public void ShowLoosePanel()
    {
        GameObject loosePanelInstance = Instantiate(loosePanel, canvas.transform);
        loosePanelInstance.transform.SetAsLastSibling();
        int fine = 1000;

        LoosePanelController.instance.SetFine(fine);
    }
}