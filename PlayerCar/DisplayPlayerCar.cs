using UnityEngine;

public class DisplayPlayerCar : MonoBehaviour
{

    [SerializeField] private GameObject vegoCar;
    [SerializeField] private GameObject x2Car;
    [SerializeField] private GameObject gtxCar;
    [SerializeField] private GameObject mozdaCar;
    [SerializeField] private GameObject microCar;


    void Awake()
    {
        DisplayCar(PlayerPrefs.GetString("currentCar", "vego"));
    }


    void Start()
    {


    }


    private void DisplayCar(string carName)
    {


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

        // ButtonControl.instance.LoadScript();
    }

}