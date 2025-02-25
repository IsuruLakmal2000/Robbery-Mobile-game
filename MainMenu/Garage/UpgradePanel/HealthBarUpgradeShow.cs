using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpgradeShow : MonoBehaviour
{
    private Slider healthBarSlider;

    public static HealthBarUpgradeShow instance;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        healthBarSlider = GetComponent<Slider>();
        healthBarSlider.maxValue = PlayerPrefs.GetInt("car_max_health", 30);
        healthBarSlider.value = PlayerPrefs.GetInt("car_max_health", 30);
    }


    public void UpdateHealthBar(int value)
    {
        int currentMaxHealth = PlayerPrefs.GetInt("car_max_health", 30);
        int newMaxHealth = currentMaxHealth + value;
        PlayerPrefs.SetInt("car_max_health", newMaxHealth);
        healthBarSlider.maxValue = newMaxHealth;
        healthBarSlider.value = newMaxHealth;

    }

    void Update()
    {
        healthBarSlider.value = PlayerPrefs.GetInt("car_max_health", 30);
    }
}