using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BusinessManager : MonoBehaviour
{
    public string businessName = "Business1"; // Unique identifier for PlayerPrefs
    public Button buyButton, upgradeButton, collectButton;
    public TextMeshProUGUI profitText, levelText, upgradeCostText, earningsText;
    [SerializeField] private GameObject profitCollectAnim;
    private int currentLevel;
    private int maxLevel = 20;
    [SerializeField] private int baseUpgradeCost = 10000;
    [SerializeField] private int baseProfit = 50000;
    [SerializeField] private int InitialCost = 50000;
    [SerializeField] private int upgradeIncreaseAmount = 10000; // Increase per level
    [SerializeField] private int profitIncreaseAmount = 25000; // Profit increase per level
    public Transform moneyBar;
    private int currentUpgradeCost;
    private int currentProfit;
    private float currentEarnings; // Earnings accumulated over time
    private float profitPerSecond;
    public Canvas canvas;
    private DateTime lastCollectedTime; // Store last collected time

    void Start()
    {
        LoadBusinessData();
        CalculateOfflineEarnings();
        UpdateUI();

        if (currentLevel > 0)
        {
            upgradeButton.gameObject.transform.parent.gameObject.SetActive(true);
            buyButton.gameObject.transform.parent.gameObject.SetActive(false);
            upgradeButton.onClick.AddListener(UpgradeBusiness);
            collectButton.onClick.AddListener(CollectEarnings);

            // Start the coroutine to calculate earnings every 10 seconds
            StartCoroutine(CalculateEarningsCoroutine());
        }
        else
        {
            upgradeButton.gameObject.transform.parent.gameObject.SetActive(false);
            buyButton.gameObject.transform.Find("price").GetComponent<TextMeshProUGUI>().text = "50k";
            buyButton.onClick.AddListener(BuyBusiness);
        }
    }

    void Update()
    {
        // if (currentLevel > 0)
        // {
        //     CalculateEarnings();
        // }
    }

    private void LoadBusinessData()
    {
        currentLevel = PlayerPrefs.GetInt($"{businessName}_level", 0);
        currentProfit = PlayerPrefs.GetInt($"{businessName}_profit", baseProfit);
        currentUpgradeCost = PlayerPrefs.GetInt($"{businessName}_upgrade_cost", baseUpgradeCost);
        currentEarnings = PlayerPrefs.GetFloat($"{businessName}_earnings", 0f);

        string lastCollectedString = PlayerPrefs.GetString($"{businessName}_lastCollectedTime", "");
        if (!string.IsNullOrEmpty(lastCollectedString))
        {
            lastCollectedTime = DateTime.Parse(lastCollectedString);
        }
        else
        {
            lastCollectedTime = DateTime.Now;
        }

        UpdateProfitPerSecond();
    }

    private void UpdateProfitPerSecond()
    {
        profitPerSecond = currentProfit / 86400f; // Convert daily profit to per second (86400 seconds in a day)
    }

    private void BuyBusiness()
    {
        if (currentLevel == 0 && CanAfford(InitialCost))
        {
            DeductMoney(InitialCost);
            currentLevel = 1;
            PlayerPrefs.SetInt($"{businessName}_level", currentLevel);
            PlayerPrefs.SetInt($"{businessName}_profit", baseProfit);
            PlayerPrefs.SetInt($"{businessName}_upgrade_cost", baseUpgradeCost);
            PlayerPrefs.SetFloat($"{businessName}_earnings", 0f);
            PlayerPrefs.SetString($"{businessName}_lastCollectedTime", DateTime.Now.ToString());

            buyButton.gameObject.transform.parent.gameObject.SetActive(false);
            upgradeButton.gameObject.transform.parent.gameObject.SetActive(true);
            upgradeButton.onClick.AddListener(UpgradeBusiness);
            UpdateProfitPerSecond();
            UpdateUI();
        }
        else
        {
            MainMenuPanelController.Instance.ShowPopupPanel("Not enough money", "You need " + FormatPrice(InitialCost) + " to buy this business");
        }
    }

    private void UpgradeBusiness()
    {

        if (currentLevel > 0 && currentLevel < maxLevel && CanAfford(currentUpgradeCost))
        {
            SoundManager.instance.PlayUpgradeSound();
            DeductMoney(currentUpgradeCost);
            currentLevel++;
            currentUpgradeCost += upgradeIncreaseAmount; // Increase cost dynamically
            currentProfit += profitIncreaseAmount; // Increase profit dynamically

            PlayerPrefs.SetInt($"{businessName}_level", currentLevel);
            PlayerPrefs.SetInt($"{businessName}_profit", currentProfit);
            PlayerPrefs.SetInt($"{businessName}_upgrade_cost", currentUpgradeCost);

            UpdateProfitPerSecond();
            UpdateUI();
        }
        else if (currentLevel == maxLevel)
        {
            //show popup that max level reached
        }
        else
        {
            //show popup that not enough money
        }
    }

    // private void CalculateEarnings()
    // {

    //     currentEarnings += Time.deltaTime * profitPerSecond;
    //     if (currentEarnings >= currentProfit)
    //     {
    //         currentEarnings = currentProfit; // Cap earnings to current profit

    //     }

    //     PlayerPrefs.SetFloat($"{businessName}_earnings", currentEarnings);

    //     //earningsText.text = currentEarnings.ToString("F2") + "/" + currentProfit.ToString() + "$";
    //     collectButton.gameObject.SetActive(currentEarnings >= 1); // Show collect button if earnings available


    // }

    private void CollectEarnings()
    {

        int earningsToCollect = Mathf.FloorToInt(currentEarnings);
        if (earningsToCollect > 0)
        {
            int playerMoney = PlayerPrefs.GetInt("total_money", 0);
            playerMoney += earningsToCollect;
            PlayerPrefs.SetInt("total_money", playerMoney);
            currentEarnings = 0;
            earningsText.text = currentEarnings.ToString("F2") + "/" + currentProfit.ToString() + "$";
            PlayerPrefs.SetFloat($"{businessName}_earnings", currentEarnings);
            PlayerPrefs.SetString($"{businessName}_lastCollectedTime", DateTime.Now.ToString());
            SoundManager.instance.PlayMoneyIncreaseSound();
            CollectMoneyWithAnimation();
            collectButton.gameObject.SetActive(false);
        }
    }

    private void CalculateOfflineEarnings()
    {
        if (currentLevel > 0)
        {
            TimeSpan timePassed = DateTime.Now - lastCollectedTime;
            float offlineEarnings = (float)timePassed.TotalSeconds * profitPerSecond;
            currentEarnings += offlineEarnings;

            PlayerPrefs.SetFloat($"{businessName}_earnings", currentEarnings);
            PlayerPrefs.SetString($"{businessName}_lastCollectedTime", DateTime.Now.ToString());

            collectButton.gameObject.SetActive(currentEarnings >= 1);
        }
    }

    private bool CanAfford(int amount)
    {
        return PlayerPrefs.GetInt("total_money", 0) >= amount;
    }
    private string FormatPrice(int price)
    {
        if (price >= 1000000) // 1M and above
            return (price / 1000000f).ToString("0.###") + "M";
        else if (price >= 1000) // 1K and above
            return (price / 1000f).ToString("0.###") + "K";
        else
            return price.ToString(); // If less than 1K, show as is
    }

    private void DeductMoney(int amount)
    {
        int playerMoney = PlayerPrefs.GetInt("total_money", 0);
        playerMoney -= amount;
        PlayerPrefs.SetInt("total_money", playerMoney);
    }

    private void UpdateUI()
    {
        levelText.text = currentLevel.ToString();
        profitText.text = "Profit/Day: $" + (currentLevel > 0 ? currentProfit.ToString("N0") : "0");
        upgradeCostText.text = (currentLevel < maxLevel ? FormatPrice(currentUpgradeCost).ToString() : "Max Level");
        earningsText.text = currentEarnings.ToString("F2") + "/" + currentProfit.ToString() + "$";
        buyButton.gameObject.SetActive(currentLevel == 0);
        upgradeButton.gameObject.SetActive(currentLevel > 0 && currentLevel < maxLevel);
        collectButton.gameObject.SetActive(currentEarnings >= 1);
    }

    public void CollectMoneyWithAnimation()
    {

        GameObject movingMoneyGroup = Instantiate(profitCollectAnim, canvas.transform);
        movingMoneyGroup.SetActive(true);

        // Get all child money icons
        List<Transform> moneyIcons = new List<Transform>();
        foreach (Transform child in movingMoneyGroup.transform)
        {
            moneyIcons.Add(child);
        }

        // Start moving each child separately
        StartCoroutine(MoveAndPackMoneyIcons(moneyIcons, moneyBar.position, movingMoneyGroup));
    }

    private IEnumerator MoveAndPackMoneyIcons(List<Transform> icons, Vector3 targetPos, GameObject parentObject)
    {
        float duration = 1f; // Total animation time
        float packTime = 0.6f; // Time to pack together
        float shrinkTime = 0.4f; // Time to shrink

        float time = 0;
        Vector3[] startPositions = new Vector3[icons.Count];

        // Store initial positions
        for (int i = 0; i < icons.Count; i++)
        {
            startPositions[i] = icons[i].position;
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            for (int i = 0; i < icons.Count; i++)
            {
                if (icons[i] != null)
                {
                    // Move towards the target
                    Vector3 packedPosition = Vector3.Lerp(startPositions[i], targetPos, t);

                    // Start packing closer together after `packTime`
                    if (time > packTime)
                    {
                        packedPosition = Vector3.Lerp(packedPosition, targetPos, (time - packTime) / shrinkTime);
                    }

                    icons[i].position = packedPosition;
                }
            }

            yield return null;
        }

        // Destroy the whole parent object once all icons reach the money bar
        Destroy(parentObject);
    }

    private IEnumerator CalculateEarningsCoroutine()
    {
        while (currentLevel > 0)
        {
            yield return new WaitForSeconds(10f); // Wait for 10 seconds

            // Calculate earnings for the last 10 seconds
            float earningsToAdd = 10f * profitPerSecond;
            currentEarnings += earningsToAdd;

            if (currentEarnings >= currentProfit)
            {
                currentEarnings = currentProfit; // Cap earnings to current profit
            }

            PlayerPrefs.SetFloat($"{businessName}_earnings", currentEarnings);

            // Update the UI
            earningsText.text = currentEarnings.ToString("F2") + "/" + currentProfit.ToString() + "$";
            collectButton.gameObject.SetActive(currentEarnings >= 1); // Show collect button if earnings are available
        }
    }
}
