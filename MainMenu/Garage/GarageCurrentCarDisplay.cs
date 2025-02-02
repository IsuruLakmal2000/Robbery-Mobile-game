using UnityEngine;

public class GarageCurrentCarDisplay : MonoBehaviour
{

    [SerializeField] private GameObject vegoCar;
    [SerializeField] private GameObject x2Car;
    [SerializeField] private GameObject gtxCar;
    [SerializeField] private GameObject mozdaCar;
    [SerializeField] private GameObject microCar;

    public static GarageCurrentCarDisplay instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        DisplayCar(PlayerPrefs.GetString("currentCar", "vego"));
    }


    public void DisplayCar(string carName)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        switch (carName)
        {
            case "vego":
                Instantiate(vegoCar, transform);
                break;
            case "x2":
                Instantiate(x2Car, transform);
                break;
            case "gtx":
                Instantiate(gtxCar, transform);
                break;
            case "mozda":
                Instantiate(mozdaCar, transform);
                break;
            case "micro":
                Instantiate(microCar, transform);
                break;
        }
    }

}