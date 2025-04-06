using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    private Button buyBtn;
    [SerializeField] private Button useBtn;
    private GameObject currentCar;
    private SetupSelectedGun setupSelectedGunScript;
    private Image backgroundColor;

    void Start()
    {
        useBtn = transform.Find("Btn").GetComponent<Button>();
        buyBtn = transform.Find("Buy Btn").GetComponent<Button>();

        bool isUnlockGun = PlayerPrefs.GetInt("is_unlock_" + gameObject.name, 0) == 1;
        backgroundColor = GetComponent<Image>();
        if (gameObject.name == PlayerPrefs.GetString("SelectedGun"))
        {
            backgroundColor.color = new Color32(60, 243, 49, 255);
        }
        else
        {
            backgroundColor.color = new Color32(170, 255, 245, 107);
        }
        currentCar = GameObject.FindGameObjectWithTag("CurrentCar");

        if (isUnlockGun)
        {
            buyBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(true);
            useBtn.onClick.AddListener(() =>
                 {
                     SoundManager.instance.PlayButtonClick();
                     ChangeGun(gameObject.name);
                 });
        }
        else
        {

            buyBtn.gameObject.SetActive(true);
            useBtn.gameObject.SetActive(false);
            DisplayGunPrice();
            buyBtn.onClick.AddListener(() =>
                 {
                     SoundManager.instance.PlayButtonClick();
                     BuyGun(gameObject.name);
                 });
        }
    }
    public void ChangeGun(string gunName)
    {
        setupSelectedGunScript = currentCar.transform.GetChild(0).GetComponent<SetupSelectedGun>();
        backgroundColor.color = new Color32(60, 243, 49, 255);
        setupSelectedGunScript.SetSelectedGun(gunName);
        PlayerPrefs.SetString("SelectedGun", gunName);

    }
    private void BuyGun(String gunName)
    {
        int price = GetGunPrice(gunName);
        int balance = PlayerPrefs.GetInt("total_money", 0);
        if (price <= balance)
        {
            balance = balance - price;

            PlayerPrefs.SetInt("total_money", balance);
            PlayerPrefs.SetInt("is_unlock_" + gunName, 1);
            buyBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(true);
            useBtn.onClick.AddListener(() =>
                 {
                     SoundManager.instance.PlayButtonClick();
                     ChangeGun(gameObject.name);
                 });
        }
        else
        {
            MainMenuPanelController.Instance.ShowPopupPanel("Not enough money", "You need more money to upgrade this item.");
            // show popup not enough money
        }

    }

    void OnGUI()
    {
        if (gameObject.name == PlayerPrefs.GetString("SelectedGun"))
        {
            backgroundColor.color = new Color32(60, 243, 49, 255);
        }
        else
        {
            backgroundColor.color = new Color32(170, 255, 245, 107);
        }
    }

    private int GetGunPrice(String gunName)
    {
        switch (gunName)
        {

            case "V1":
                return 10000;
            case "V2":
                return 30000;
            case "V3":
                return 500000;
            case "V4":
                return 750000;
            case "V5":
                return 100000;
            default:
                return 0;

        }
    }
    private void DisplayGunPrice()
    {
        int price = GetGunPrice(gameObject.name);
        string formattedPrice = FormatPrice(price);
        buyBtn.transform.Find("price").GetComponent<TextMeshProUGUI>().text = formattedPrice.ToString();
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