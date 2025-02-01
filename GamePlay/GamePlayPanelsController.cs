using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelsController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    public static GamePlayPanelsController instance;
    private Canvas canvas;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        canvas = FindObjectOfType<Canvas>();

    }


    public void ShowWinPanel()
    {
        GameObject winPanelInstance = Instantiate(winPanel, canvas.transform);
        winPanelInstance.transform.SetAsLastSibling();
        int totalMoneyInthisLevel = GameRobbedMoney.instance.robbedMoneyCount;
        WinPanelController.instance.SetRobbedMoney(totalMoneyInthisLevel);
        PlayerPrefs.SetInt("total_money", PlayerPrefs.GetInt("total_money", 0) + totalMoneyInthisLevel);

    }
}