using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MillionareClubController : MonoBehaviour
{
    [SerializeField] private Button buyBtn;
    [SerializeField] private TextMeshProUGUI profitAmount;
    [SerializeField] private TextMeshProUGUI currentEarningTxt;
    [SerializeField] private GameObject profitAvailableVBtn;
    [SerializeField] private GameObject profitCollectAnim;

    private const int DAILY_LIMIT = 10000000; // 10M
    private const int EARN_PER_MINUTE = DAILY_LIMIT / (24 * 60); // Earnings per minute
    private const int EARN_PER_SECOND = EARN_PER_MINUTE / 60; // Earnings per second
    private const int EARN_PER_TEN_SECONDS = EARN_PER_SECOND * 10; // Earnings every 10 seconds
    private int currentEarnings = 0; // Changed from double to int
    private DateTime lastCollectTime;

    public Transform moneyBar; // Assign the UI money bar (target position)
    public Canvas canvas;

    void Start()
    {
        CheckUnlockMillionaire();
    }
    private void CheckUnlockMillionaire()
    {
        int currentMoney = PlayerPrefs.GetInt("total_money", 0);
        int totalGem = PlayerPrefs.GetInt("total_gem", 0);
        profitAmount.text = FormatPrice(10000000) + "/per day income";
        if (PlayerPrefs.GetInt("is_unlock_millionare_club", 0) == 1)
        {

            currentEarningTxt.gameObject.SetActive(true);
            // Retrieve the last collection time and calculate the current earnings
            if (PlayerPrefs.HasKey("LastCollectTime_millionare_club"))
            {
                lastCollectTime = DateTime.FromBinary(Convert.ToInt64(PlayerPrefs.GetString("LastCollectTime_millionare_club")));
                // Calculate the earnings based on the time elapsed since the last collection
                TimeSpan timeSinceLastCollect = DateTime.Now - lastCollectTime;
                int secondsToCalculate = Mathf.Min((int)timeSinceLastCollect.TotalSeconds, 24 * 60 * 60); // Cap to max of 24 hours
                currentEarnings = secondsToCalculate * EARN_PER_SECOND;
                currentEarningTxt.text = FormatPrice(currentEarnings) + " / " + FormatPrice(DAILY_LIMIT);
                InvokeRepeating(nameof(UpdateEarnings), 1f, 1f);
            }
            else
            {
                lastCollectTime = DateTime.Now;
                PlayerPrefs.SetString("LastCollectTime_millionare_club", lastCollectTime.ToBinary().ToString());
                TimeSpan timeSinceLastCollect = DateTime.Now - lastCollectTime;
                int secondsToCalculate = Mathf.Min((int)timeSinceLastCollect.TotalSeconds, 24 * 60 * 60); // Cap to max of 24 hours
                currentEarnings = secondsToCalculate * EARN_PER_SECOND;
                currentEarningTxt.text = FormatPrice(currentEarnings) + " / " + FormatPrice(DAILY_LIMIT);
                InvokeRepeating(nameof(UpdateEarnings), 1f, 1f);
            }

            buyBtn.gameObject.SetActive(false);
        }
        else
        {
            profitAvailableVBtn.SetActive(false);
            currentEarningTxt.gameObject.SetActive(false);
            buyBtn.onClick.AddListener(() =>
            {
                SoundManager.instance.PlayButtonClick();
                if (currentMoney >= 1000000)
                {
                    if (totalGem >= 100)
                    {
                        SoundManager.instance.PlayUpgradeSound();
                        totalGem -= 100;
                        PlayerPrefs.SetInt("total_gem", totalGem);
                        currentMoney -= 1000000;
                        PlayerPrefs.SetInt("total_money", currentMoney);

                        PlayerPrefs.SetInt("is_unlock_millionare_club", 1);
                        buyBtn.gameObject.SetActive(false);
                        currentEarningTxt.gameObject.SetActive(true);
                        CheckUnlockMillionaire();
                    }
                    else
                    {
                        MainMenuPanelController.Instance.ShowPopupPanel("Not enough gem", "You need 100 gem to join Millionare Club");
                    }

                }
                else
                {
                    MainMenuPanelController.Instance.ShowPopupPanel("Not enough money", "You need " + FormatPrice(1000000) + " to join Millionare Club");
                }
            });
        }

    }

    void UpdateEarnings()
    {
        TimeSpan timeSinceLastCollect = DateTime.Now - lastCollectTime;

        // Cap time calculation to 24 hours max

        int secondsToCalculate = Mathf.Min((int)timeSinceLastCollect.TotalSeconds, 24 * 60 * 60);

        int newEarnings = secondsToCalculate * EARN_PER_SECOND;


        currentEarnings = Mathf.Min(newEarnings, DAILY_LIMIT);

        // Update the UI to display the current earnings
        currentEarningTxt.text = FormatPrice(currentEarnings) + " / " + FormatPrice(DAILY_LIMIT);

        // Show popup if money is available
        if (currentEarnings > 0)
        {
            profitAvailableVBtn.SetActive(true);
            profitAvailableVBtn.GetComponent<Button>().onClick.AddListener(CollectMoney);
        }



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

    public void CollectMoney()
    {
        if (currentEarnings > 0)
        {
            // Give player the earnings
            int collectedAmount = currentEarnings;
            int currentTotalMoney = PlayerPrefs.GetInt("total_money", 0);
            PlayerPrefs.SetInt("total_money", currentTotalMoney + collectedAmount);
            Debug.Log($"Collected: {collectedAmount}");

            // Reset earnings and update last collect time
            currentEarnings = 0;
            lastCollectTime = DateTime.Now;
            profitAvailableVBtn.SetActive(false);
            currentEarningTxt.text = FormatPrice(currentEarnings) + " / " + FormatPrice(DAILY_LIMIT);
            // Save state
            PlayerPrefs.SetString("LastCollectTime_millionare_club", lastCollectTime.ToBinary().ToString());
            //  PlayerPrefs.SetInt("CurrentEarnings", currentEarnings); // Store as int
            PlayerPrefs.Save();
            SoundManager.instance.PlayMoneyIncreaseSound();
            CollectMoneyWithAnimation();
            //  UpdateUI();
        }
    }

    void UpdateUI()
    {
        // Update UI with formatted profit amount
        // profitAmount.text = FormatPrice(currentEarnings);
    }

    public void CollectMoneyWithAnimation()
    {
        // Duplicate the entire animation object
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
}
