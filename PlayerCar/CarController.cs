using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public float laneChangeSpeed = 5f; // Speed at which the car changes lanes
    public float maxLaneOffset = 1.5f; // Maximum distance the car can drift left or right
    private bool isMovingOffScreen = false;
    public float offScreenSpeed = 10f;
    private float currentLaneOffset = 0f;
    private float input = 0f;
    [SerializeField] private GameObject vehicleDamageVfx;
    [SerializeField] private AudioSource audioSourceOnPlayerCar;
    [SerializeField] private GameObject explosionVfx;
    [SerializeField] private GameObject moneyIncreaseEffect;
    [SerializeField] private GameObject gemIncreaseEffect;
    //---------------------sounds----------------------
    [SerializeField] private AudioClip PlayerDestroySound;
    [SerializeField] private AudioClip PlayerCarHitSound;
    [SerializeField] private AudioClip CarTurningSound;
    public static CarController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        if (!isMovingOffScreen)
        {
            HandleLaneChange();
        }
        else
        {
            MoveCarOffScreen();
        }

        // Apply the lane offset to the car's position
        Vector3 targetPosition = new Vector3(currentLaneOffset, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneChangeSpeed * Time.deltaTime);
    }

    private void PlayCarSound(AudioClip audioClip, float volume)
    {
        audioSourceOnPlayerCar.clip = audioClip;
        audioSourceOnPlayerCar.volume = 0.3f;
        audioSourceOnPlayerCar.loop = false;
        audioSourceOnPlayerCar.Play();
    }

    private void HandleLaneChange()
    {
        // Update currentLaneOffset based on input
        if (input != 0f)
        {
            currentLaneOffset += input * laneChangeSpeed * Time.deltaTime;

            // Clamp the offset to the maximum allowed distance
            currentLaneOffset = Mathf.Clamp(currentLaneOffset, -maxLaneOffset, maxLaneOffset);

            // Rotate the car based on lane change direction
            if (currentLaneOffset != maxLaneOffset && currentLaneOffset != -maxLaneOffset)
            {
                PlayCarSound(CarTurningSound, 0.2f);
                float targetRotationZ = input * -20f; // Rotate left or right
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotationZ), Time.deltaTime * 8f);
            }


        }
        else
        {
            // Reset the car rotation back to the original position when input is zero
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    // Method to set input value
    public void SetInput(float value)
    {
        input = value;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameRobbedMoney.instance.ReduceMoneyWhenHit(200);
        if (collision.gameObject.CompareTag("OtherCar"))
        {

            ContactPoint2D contact = collision.contacts[0];
            GameObject vfxInstance = Instantiate(vehicleDamageVfx, contact.point, Quaternion.identity);
            Destroy(vfxInstance, 1f);
            if (HealthBarController.instance.CheckHealthEnd())
            {
                PlayCarSound(PlayerDestroySound, 1f);
                Instantiate(explosionVfx, transform);
                SoundManager.instance.PlayVehicleDestroyedSound();
                SoundManager.instance.PlayLevelFailedSound();
                Destroy(gameObject, 0.3f);

                GamePlayPanelsController.instance.ShowLoosePanel();
            }
            else
            {
                HealthBarController.instance.ReduceHealth(4);
            }

        }
        if (collision.gameObject.CompareTag("PoliceCar"))
        {
            if (HealthBarController.instance.CheckHealthEnd())
            {
                PlayCarSound(PlayerDestroySound, 1f);
                Instantiate(explosionVfx, transform);
                SoundManager.instance.PlayVehicleDestroyedSound();
                SoundManager.instance.PlayLevelFailedSound();
                Destroy(gameObject, 0.3f);

                GamePlayPanelsController.instance.ShowLoosePanel();
            }
            else
            {
                HealthBarController.instance.ReduceHealth(4);
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            SoundManager.instance.PlayMoneyPickupSound();
            GameRobbedMoney.instance.IncreaseMoneyWhenCollect(100);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            GameObject moneyIncreaseEffectInstance = Instantiate(moneyIncreaseEffect, screenPos, Quaternion.identity, transform.Find("Canvas").transform);
            moneyIncreaseEffectInstance.GetComponent<MoneyPopup>().Setup(100);
          
        }
        if (collision.gameObject.CompareTag("Gem"))
        {
            SoundManager.instance.PlayMoneyPickupSound();
            GameRobbedMoney.instance.IncreaseGemWhenCollect(1);
            GameObject moneyIncreaseEffectInstance = Instantiate(gemIncreaseEffect, transform.Find("Canvas").transform);
            Destroy(moneyIncreaseEffectInstance, 0.5f);
        }
        if (collision.gameObject.CompareTag("AdMoney"))
        {
            SoundManager.instance.PlayAlertInfoSound();
            string rewardTxt = collision.gameObject.transform.Find("Canvas/rewardCount").gameObject.GetComponent<TextMeshProUGUI>().text;
            Debug.Log("reard texxt --" + rewardTxt);
            int exactPrice = ParseFormattedPrice(rewardTxt);
            Debug.Log("rewarded money on ad money === " + exactPrice);
            GamePlayPanelsController.instance.ShowWatchAddPopup(exactPrice, "watch ad ?", "watch add to collect this money", "watch_ad_money_gameplay");
            Time.timeScale = 0f;
        }
    }

    private void MoveCarOffScreen()
    {
        // Move the car upwards until it is off the camera
        transform.Translate(Vector3.up * offScreenSpeed * Time.deltaTime);

        // Optionally, you can check if the car is off the camera and stop the movement
        if (Camera.main != null)
        {
            if (transform.position.y > Camera.main.orthographicSize + 1) // Adjust as needed
            {
                // Time.timeScale = 0f;
                isMovingOffScreen = false; // Stop moving
            }
        }
    }

    // Method to trigger moving off-screen
    public void CompleteLevel()
    {
        isMovingOffScreen = true;


    }

    private int ParseFormattedPrice(string formattedPrice)
    {
        formattedPrice = formattedPrice.Trim();
        Debug.Log("FORMATTED PRICE -----" + formattedPrice);
        if (formattedPrice.EndsWith("M"))
        {
            // Remove "M" and parse as a million
            if (double.TryParse(formattedPrice.Substring(0, formattedPrice.Length - 1), out double value))
            {
                return (int)(value * 1000000);
            }
        }
        else if (formattedPrice.EndsWith("K"))
        {
            // Remove "K" and parse as a thousand
            if (double.TryParse(formattedPrice.Substring(0, formattedPrice.Length - 1), out double value))
            {
                return (int)(value * 1000);
            }
        }
        else
        {
            // Parse as a regular number
            if (int.TryParse(formattedPrice, out int value))
            {
                return value;
            }
        }

        // Return 0 if parsing fails
        return 0;
    }

}