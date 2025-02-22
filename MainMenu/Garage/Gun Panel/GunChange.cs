using UnityEngine;
using UnityEngine.UI;

public class GunChange : MonoBehaviour
{
    private Button buyBtn;
    private GameObject currentCar;
    private SetupSelectedGun setupSelectedGunScript;
    private Image backgroundColor;

    void Start()
    {
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

        buyBtn = transform.Find("Btn").GetComponent<Button>();
        buyBtn.onClick.AddListener(() => ChangeGun(gameObject.name));
    }
    public void ChangeGun(string gunName)
    {
        setupSelectedGunScript = currentCar.transform.GetChild(0).GetComponent<SetupSelectedGun>();
        backgroundColor.color = new Color32(60, 243, 49, 255);
        setupSelectedGunScript.SetSelectedGun(gunName);
        PlayerPrefs.SetString("SelectedGun", gunName);

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
}