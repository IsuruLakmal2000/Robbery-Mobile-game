using System;
using UnityEngine;

public class SetupSelectedGun : MonoBehaviour
{


    [SerializeField] private GameObject v1Prefab;
    [SerializeField] private GameObject v2Prefab;
    [SerializeField] private GameObject v3Prefab;
    [SerializeField] private GameObject v4Prefab;

    [SerializeField] private GameObject gunPoint;
    public static SetupSelectedGun instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        String selectedGun = PlayerPrefs.GetString("SelectedGun", "none");

        switch (selectedGun)
        {
            case "V1":
                Instantiate(v1Prefab, gunPoint.transform);
                break;
            case "V2":
                Instantiate(v2Prefab, gunPoint.transform);
                break;
            case "V3":
                Instantiate(v3Prefab, gunPoint.transform);
                break;
            case "V4":
                Instantiate(v4Prefab, gunPoint.transform);
                break;
        }
    }

    public void SetSelectedGun(String gunName)
    {
        if (gunPoint.transform.childCount > 0)
        {
            Destroy(gunPoint.transform.GetChild(0).gameObject);
        }


        PlayerPrefs.SetString("SelectedGun", gunName);

        switch (gunName)
        {
            case "V1":
                Instantiate(v1Prefab, gunPoint.transform);
                break;
            case "V2":
                Instantiate(v2Prefab, gunPoint.transform);
                break;
            case "V3":
                Instantiate(v3Prefab, gunPoint.transform);
                break;
            case "V4":
                Instantiate(v4Prefab, gunPoint.transform);
                break;
        }
    }
}