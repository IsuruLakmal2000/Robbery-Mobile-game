using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonProp : MonoBehaviour
{

    private Button upgradeButton;
    private TextMeshProUGUI priceTxt;
    private TextMeshProUGUI levelTxt;

    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel = 20;
    [SerializeField] private int basePrice = 1000;
    [SerializeField] private float priceMultiplier = 1.2f; // Price increases by 20%
    void Start()
    {
        priceTxt = transform.Find("price").GetComponent<TextMeshProUGUI>();
        levelTxt = transform.parent.Find("Level/levelTxt").GetComponent<TextMeshProUGUI>();

        upgradeButton = GetComponent<Button>();
        upgradeButton.onClick.AddListener(Upgrade);
        LoadUpgradeData();
        levelTxt.text = currentLevel.ToString();
    }
    private void LoadUpgradeData()
    {
        currentLevel = PlayerPrefs.GetInt(gameObject.name + "_Level", 1);
        int price = PlayerPrefs.GetInt(gameObject.name + "_Price", basePrice);

        priceTxt.text = FormatPrice(price);
        CheckUpgradeAvailability();
    }
    private void CheckUpgradeAvailability()
    {
        if (currentLevel >= maxLevel)
        {
            upgradeButton.interactable = false;
            priceTxt.text = "Max";
        }
    }

    void OnGUI()
    {
        levelTxt.text = currentLevel.ToString();
    }

    private void Upgrade()
    {
        Debug.Log("Upgrade");
        if (currentLevel >= maxLevel)
            return; // Max level reached

        int price = PlayerPrefs.GetInt(gameObject.name + "_Price", basePrice);

        // Check if the player has enough currency (Implement your own currency check here)
        if (!HasEnoughMoney(price))
        {
            Debug.Log("Not enough money");
            //dont have money
            SoundManager.instance.PlayAlertWarnSound();
            MainMenuPanelController.Instance.ShowPopupPanel("Not enough money", "You need more money to upgrade this item.");
            return;
        }


        SpendMoney(price);

        // Perform upgrade
        switch (gameObject.name)
        {
            case "HealthUpgradeButton":
                SoundManager.instance.PlayUpgradeSound();
                HealthBarUpgradeShow.instance.UpdateHealthBar(5);
                break;
            case "SpeedUpgradeButton":
                // SoundManager.instance.PlayUpgradeSound();
                //  SpeedBarUpgradeShow.instance.UpdateSpeedBar(10);
                break;
            case "DamageUpgradeButton":
                // SoundManager.instance.PlayUpgradeSound();
                //  DamageBarUpgradeShow.instance.UpdateDamageBar(10);
                break;
        }

        // Increase level
        currentLevel++;
        PlayerPrefs.SetInt(gameObject.name + "_Level", currentLevel);

        // Increase price
        price = Mathf.RoundToInt(price * priceMultiplier);
        PlayerPrefs.SetInt(gameObject.name + "_Price", price);

        priceTxt.text = FormatPrice(price);
        CheckUpgradeAvailability();
    }

    private bool HasEnoughMoney(int price)
    {
        int playerMoney = PlayerPrefs.GetInt("total_money", 0);
        return playerMoney >= price;
    }

    private void SpendMoney(int amount)
    {

        int playerMoney = PlayerPrefs.GetInt("total_money", 0);
        PlayerPrefs.SetInt("total_money", playerMoney - amount);
    }
    private string FormatPrice(int price)
    {
        if (price >= 1000000) // 1M and above
            return (price / 1000000f).ToString("0.#") + "M";
        else if (price >= 1000) // 1K and above
            return (price / 1000f).ToString("0.#") + "K";
        else
            return price.ToString(); // If less than 1K, show as is
    }
}