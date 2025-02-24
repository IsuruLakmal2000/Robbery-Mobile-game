using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class VehicleChange : MonoBehaviour
{
    [SerializeField] private Button useBtn;
    [SerializeField] private Button buyBtn;
    private Image backgroundColor;

    void Start()
    {
        useBtn = transform.Find("Btn").GetComponent<Button>();
        buyBtn = transform.Find("Buy Btn").GetComponent<Button>();
        backgroundColor = GetComponent<Image>();
        if (gameObject.name == PlayerPrefs.GetString("currentCar"))
        {
            backgroundColor.color = new Color32(60, 243, 49, 255);
        }
        else
        {
            backgroundColor.color = new Color32(170, 255, 245, 107);
        }
        bool isUnlockVehicle = PlayerPrefs.GetInt("is_unlock_" + gameObject.name, 0) == 1;
        if (isUnlockVehicle)
        {
            buyBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(true);
            useBtn.onClick.AddListener(() =>
                 {
                     ChangeVehicle(gameObject.name);
                 });
        }
        else
        {

            buyBtn.gameObject.SetActive(true);
            useBtn.gameObject.SetActive(false);
            DisplayVehiclePrice();
            buyBtn.onClick.AddListener(() =>
                 {
                     BuyVehicle(gameObject.name);
                 });
        }


    }

    private void BuyVehicle(String vehicleName)
    {
        int price = GetVehiclePrice(vehicleName);
        int balance = PlayerPrefs.GetInt("total_money", 0);
        if (price <= balance)
        {
            balance = balance - price;
            PlayerPrefs.SetInt("total_money", balance);
            PlayerPrefs.SetInt("is_unlock_" + vehicleName, 1);
            buyBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(true);
            useBtn.onClick.AddListener(() =>
                 {
                     ChangeVehicle(gameObject.name);
                 });
        }
        else
        {
            // show popup not enough money
        }

    }

    private int GetVehiclePrice(String vehicleName)
    {
        switch (vehicleName)
        {

            case "vego":
                return 10000;
            case "x2":
                return 30000;
            case "gtx":
                return 100000;
            case "mozda":
                return 200000;
            case "micro":
                return 500000;
            default:
                return 0;

        }
    }

    private void DisplayVehiclePrice()
    {
        int price = GetVehiclePrice(gameObject.name);
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
    public void ChangeVehicle(string carName)
    {

        backgroundColor.color = new Color32(60, 243, 49, 255);
        GarageCurrentCarDisplay.instance.DisplayCar(carName);
        PlayerPrefs.SetString("currentCar", carName);

    }

    void OnGUI()
    {
        if (gameObject.name == PlayerPrefs.GetString("currentCar"))
        {
            backgroundColor.color = new Color32(60, 243, 49, 255);
        }
        else
        {
            backgroundColor.color = new Color32(170, 255, 245, 107);
        }
    }
}