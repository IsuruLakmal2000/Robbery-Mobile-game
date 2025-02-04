using UnityEngine;
using UnityEngine.UI;

public class VehicleChange : MonoBehaviour
{
    private Button buyBtn;

    void Start()
    {
        buyBtn = transform.Find("Btn").GetComponent<Button>();
        buyBtn.onClick.AddListener(() => ChangeVehicle(gameObject.name));
    }
    public void ChangeVehicle(string carName)
    {
        GarageCurrentCarDisplay.instance.DisplayCar(carName);
        PlayerPrefs.SetString("currentCar", carName);

    }
}