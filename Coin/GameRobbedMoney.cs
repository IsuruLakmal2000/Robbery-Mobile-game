using TMPro;
using UnityEngine;

public class GameRobbedMoney : MonoBehaviour
{
    public int robbedMoneyCount = 0;
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
        robbedMoneyCount = robbedMoneyCount - count;
    }
    public void IncreaseMoneyWhenCollect(int count)
    {
        robbedMoneyCount = robbedMoneyCount + count;
    }
    private void OnGUI()
    {
        coinCount.text = robbedMoneyCount.ToString();
    }

}