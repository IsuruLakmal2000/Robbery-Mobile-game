using TMPro;
using UnityEngine;

public class GameRobbedMoney : MonoBehaviour
{
    public int robbedMoneyCount = 0;
    public int gameCollectedGemCount = 0;
    [SerializeField] private TextMeshProUGUI coinCount;

    public static GameRobbedMoney instance;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        robbedMoneyCount = GameManager.instance.levelConfig.robbedMoney;

        coinCount.text = robbedMoneyCount.ToString();
    }
    public void ReduceMoneyWhenHit(int count)
    {
        if (robbedMoneyCount - count < 0)
        {
            robbedMoneyCount = 0;
            return;
        }
        robbedMoneyCount = robbedMoneyCount - count;
    }
    public void IncreaseMoneyWhenCollect(int count)
    {
        robbedMoneyCount = robbedMoneyCount + count;
    }
    public void IncreaseGemWhenCollect(int count)
    {
        gameCollectedGemCount = gameCollectedGemCount + count;
    }
    private void OnGUI()
    {
        coinCount.text = robbedMoneyCount.ToString();
    }

}