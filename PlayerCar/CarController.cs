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
    [SerializeField] private GameObject explosionVfx;
    [SerializeField] private GameObject moneyIncreaseEffect;
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

    private void HandleLaneChange()
    {
        // Update currentLaneOffset based on input
        if (input != 0f)
        {
            currentLaneOffset += input * laneChangeSpeed * Time.deltaTime;

            // Clamp the offset to the maximum allowed distance
            currentLaneOffset = Mathf.Clamp(currentLaneOffset, -maxLaneOffset, maxLaneOffset);

            // Rotate the car based on lane change direction
            float targetRotationZ = input * -20f; // Rotate left or right
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetRotationZ), Time.deltaTime * 8f);
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

                Instantiate(explosionVfx, transform);
                Destroy(gameObject, 0.3f);
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

                Instantiate(explosionVfx, transform);
                Destroy(gameObject, 0.3f);
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

            GameRobbedMoney.instance.IncreaseMoneyWhenCollect(100);
            GameObject moneyIncreaseEffectInstance = Instantiate(moneyIncreaseEffect, transform.Find("Canvas").transform);
            Destroy(moneyIncreaseEffectInstance, 0.5f);
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

}