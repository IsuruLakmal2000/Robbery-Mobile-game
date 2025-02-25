using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUpgradeShow : MonoBehaviour
{
    private Slider healthBarSlider;
    private TextMeshProUGUI currentHealthTxt;
    private TextMeshProUGUI maxHealthTxt;

    public static HealthBarUpgradeShow instance;
    int currentMaxHealth = 30;
    int newMaxHealth = 30;
    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        currentHealthTxt = transform.Find("CurrentHealth").GetComponent<TextMeshProUGUI>();
        maxHealthTxt = transform.Find("MaxHealth").GetComponent<TextMeshProUGUI>();
        healthBarSlider = GetComponent<Slider>();
        currentMaxHealth = PlayerPrefs.GetInt("car_max_health", 30);
        healthBarSlider.maxValue = currentMaxHealth;
        healthBarSlider.value = currentMaxHealth;
        currentHealthTxt.text = currentMaxHealth.ToString();
        maxHealthTxt.text = "/ " + currentMaxHealth.ToString();



    }


    public void UpdateHealthBar(int value)
    {
        Debug.Log("health upgrad Value: " + value);
        currentMaxHealth = PlayerPrefs.GetInt("car_max_health", 30);
        newMaxHealth = currentMaxHealth + value;
        currentMaxHealth = newMaxHealth;
        PlayerPrefs.SetInt("car_max_health", newMaxHealth);
        healthBarSlider.maxValue = newMaxHealth;
        healthBarSlider.value = newMaxHealth;

    }

    void Update()
    {

        currentHealthTxt.text = currentMaxHealth.ToString();
        maxHealthTxt.text = "/ " + currentMaxHealth.ToString();
    }
}