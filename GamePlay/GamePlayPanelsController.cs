using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelsController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
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
}