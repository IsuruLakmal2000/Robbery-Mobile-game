using System;
using UnityEngine;

public class SetupSelectedGun : MonoBehaviour
{


    [SerializeField] private GameObject v1Prefab;
    [SerializeField] private GameObject v2Prefab;
    [SerializeField] private GameObject v3Prefab;
    [SerializeField] private GameObject v4Prefab;

    [SerializeField] private GameObject gunPoint;
    [SerializeField] private GameObject secondaryGunPoint;
    public static SetupSelectedGun instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        String selectedGun = PlayerPrefs.GetString("SelectedGun", "none");
        if (selectedGun != "none")
        {
            Instantiate(v1Prefab, gunPoint.transform);
        }
        String SecondaryGun = PlayerPrefs.GetString("SecondaryGun", "none");
        if (SecondaryGun != "none")
        {
            Instantiate(v2Prefab, secondaryGunPoint.transform);
        }
        // switch (selectedGun)
        // {
        //     case "V1":
        //         Instantiate(v1Prefab, gunPoint.transform);
        //         break;
        //     case "V2":
        //         Instantiate(v2Prefab, secondaryGunPoint.transform);
        //         break;
        //     case "V3":
        //         Instantiate(v3Prefab, gunPoint.transform);
        //         break;
        //     case "V4":
        //         Instantiate(v4Prefab, gunPoint.transform);
        //         break;
        // }
    }
    public void SetupSecondaryGun(String gunName)
    {
        if (secondaryGunPoint.transform.childCount > 0)
        {
            Destroy(secondaryGunPoint.transform.GetChild(0).gameObject);
        }
        Instantiate(v2Prefab, secondaryGunPoint.transform);
    }

    public void SetSelectedGun(String gunName)
    {
        if (gunPoint.transform.childCount > 0)
        {
            Destroy(gunPoint.transform.GetChild(0).gameObject);
        }

        Instantiate(v1Prefab, gunPoint.transform);
        // PlayerPrefs.SetString("SelectedGun", gunName);

        // switch (gunName)
        // {
        //     case "V1":
        //         Instantiate(v1Prefab, gunPoint.transform);
        //         break;
        //     case "V2":
        //         Instantiate(v2Prefab, gunPoint.transform);
        //         break;
        //     case "V3":
        //         Instantiate(v3Prefab, gunPoint.transform);
        //         break;
        //     case "V4":
        //         Instantiate(v4Prefab, gunPoint.transform);
        //         break;
        // }
    }
}