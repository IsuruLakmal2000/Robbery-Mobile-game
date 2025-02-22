using UnityEngine;
using UnityEngine.UI;

public class VehicleChange : MonoBehaviour
{
    private Button buyBtn;
    private Image backgroundColor;

    void Start()
    {
        backgroundColor = GetComponent<Image>();
        if (gameObject.name == PlayerPrefs.GetString("currentCar"))
        {
            backgroundColor.color = new Color32(60, 243, 49, 255);
        }
        else
        {
            backgroundColor.color = new Color32(170, 255, 245, 107);
        }

        buyBtn = transform.Find("Btn").GetComponent<Button>();
        buyBtn.onClick.AddListener(() => ChangeVehicle(gameObject.name));
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