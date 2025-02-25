using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{

    [SerializeField] Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI currentHealthCountTxt;
    [SerializeField] private TextMeshProUGUI maxHealthCountTxt;
    private float currentCarHealth = 50;
    private float maxCarHealth = 50;


    public static HealthBarController instance;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        maxCarHealth = PlayerPrefs.GetInt("car_max_health", 30);
        healthBarSlider.maxValue = maxCarHealth;
        healthBarSlider.value = maxCarHealth;
        maxHealthCountTxt.text = "/ " + maxCarHealth.ToString();

    }
    public bool CheckHealthEnd()
    {
        if (currentCarHealth == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ReduceHealth(int count)
    {
        if (currentCarHealth <= count)
        {
            currentCarHealth = 0;
        }
        else
        {
            currentCarHealth = currentCarHealth - count;
        }

    }

    private void Update()
    {

    }
    private void OnGUI()
    {
        healthBarSlider.value = currentCarHealth;
        currentHealthCountTxt.text = currentCarHealth.ToString();

    }
}