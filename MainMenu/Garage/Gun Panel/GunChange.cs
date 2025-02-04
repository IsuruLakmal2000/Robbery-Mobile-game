using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    private Button buyBtn;
    private GameObject currentCar;
    private SetupSelectedGun setupSelectedGunScript;

    void Start()
    {
        currentCar = GameObject.FindGameObjectWithTag("CurrentCar");

        buyBtn = transform.Find("Btn").GetComponent<Button>();
        buyBtn.onClick.AddListener(() => ChangeGun(gameObject.name));
    }
    public void ChangeGun(string gunName)
    {
        setupSelectedGunScript = currentCar.transform.GetChild(0).GetComponent<SetupSelectedGun>();

        setupSelectedGunScript.SetSelectedGun(gunName);
        PlayerPrefs.SetString("SelectedGun", gunName);

    }
}