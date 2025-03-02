using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PoliceCarController : MonoBehaviour
{
    private bool isMovingUncontrollably = false;
    private Quaternion originalRotation; // Store the original rotation
    public Transform playerCar; // Reference to the player's car
    public float followDistance = 2f; // Desired distance behind the player's car
    public float speed = 4f; // Speed of the police car
    public float hitDistance = 0.5f; // Distance to trigger collision
    public float resetDistance = 5f; // Distance to reset the police car position after hitting
    public float passDistance = 1.5f; // Distance to pass the player car
    public float collisionChance = 0.3f; // Chance of attempting to collide with the player car
    public float laneChangeChance = 0.05f; // Chance of changing lanes
    public float laneWidth = 1f; // Width of the lanes for lane changes
    private float targetLaneX; // Target x position for lane
    public Slider healthBar; // Assign this in the Inspector
    public float maxHealth = 10f;
    private float currentHealth;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private int destroyEarning = 500;

    [SerializeField] private GameObject exPlosionPrefab;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject blueLight;
    [SerializeField] private GameObject moneyBoomExplosionPrefab;
    // [SerializeField] private GameObject moneyIncreaseEffect;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        // Store the original rotation
        originalRotation = transform.rotation;
        StartCoroutine(BlinkLights());
        // Find the player car by tag if not assigned
        if (playerCar == null)
        {
            GameObject playerCarObject = GameObject.FindWithTag("PlayerCar");
            if (playerCarObject != null)
            {
                playerCar = playerCarObject.transform; // Get the Transform component
            }
            else
            {
                return;

            }
        }

        // Set initial lane position based on player's position
        targetLaneX = GetLaneXPosition();
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }



    private void Update()
    {
        if (playerCar == null)
        {
            // Calculate the movement direction based on the vehicle's rotation
            Vector3 moveDirection = Vector3.down;
            // Normalize the direction to maintain speed
            moveDirection.Normalize();
            transform.position += moveDirection * speed * 2 * Time.deltaTime; // Move the vehicle in the determined direction

            // Optionally, add logic to stop moving uncontrollably after a certain time or condition
        }
        else
        {
            FollowPlayer();
        }


        // Smoothly rotate back to the original rotation if needed
        if (Quaternion.Angle(transform.rotation, originalRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, 200 * Time.deltaTime);
        }
    }

    private void FollowPlayer()
    {
        // Calculate target position directly behind the player with lane adjustment
        Vector3 targetPosition = new Vector3(playerCar.position.x, playerCar.position.y - followDistance, playerCar.position.z);
        targetPosition.x = targetLaneX; // Ensure it stays in the target lane

        // Check for collision with the player's car
        if (Vector3.Distance(transform.position, playerCar.position) < hitDistance)
        {
            // Handle collision logic if needed
        }

        // Smoothly move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Randomly decide to change lanes with lower frequency
        if (Random.value < laneChangeChance)
        {
            ChangeLane();
        }

        // Check if the police car should attempt to pass the player car
        if (Vector3.Distance(transform.position, playerCar.position) < passDistance)
        {
            PassPlayer();
        }

        // Implement avoidance logic to avoid other cars
        AvoidOtherCars();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCar"))
        {
            //   HandleCollisionWithPlayer(collision);
        }

        if (collision.gameObject.CompareTag("Area"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1f);
            GameObject hitEffect = Instantiate(hitPrefab, transform);
            Destroy(hitEffect, 1f);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthBar.value = currentHealth;

        if (currentHealth <= 0)
        {
            // GameObject moneyIncreaseVfx = Instantiate(moneyIncreaseEffect, canvas.transform);
            LevelManager.instance.totalDestriyedPoliceVehiclesCount++;
            GameObject explosion = Instantiate(exPlosionPrefab, transform.position, Quaternion.identity);
            GameObject moneyBoomExplosion = Instantiate(moneyBoomExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(moneyBoomExplosion, 1f);
            // Destroy(moneyIncreaseVfx, 1f);
            Destroy(explosion, 1f);
            // Time.timeScale = 0f;
            GameRobbedMoney.instance.IncreaseMoneyWhenCollect(destroyEarning);
            Destroy(gameObject);

        }
    }


    private void HandleCollisionWithPlayer(Collision2D collision)
    {
        // Store the original rotation before applying randomness
        originalRotation = transform.rotation;

        // Randomly rotate the vehicle upon collision
        float randomAngle = Random.Range(-30f, 30f); // Adjust the range as needed
        Quaternion targetRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + randomAngle);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);

        // Set the vehicle to move uncontrollably
        isMovingUncontrollably = true; // Indicate that the vehicle is now moving in a new direction
    }

    private void ChangeLane()
    {
        // Choose a new lane position
        float newLaneX = targetLaneX + (Random.value < 0.5f ? -laneWidth : laneWidth);
        targetLaneX = Mathf.Clamp(newLaneX, -3f, 3f); // Adjust boundaries as needed
    }

    private void PassPlayer()
    {
        // Move the police car upwards to pass the player car
        Vector3 passPosition = new Vector3(playerCar.position.x, playerCar.position.y + followDistance, playerCar.position.z);
        transform.position = Vector3.MoveTowards(transform.position, passPosition, speed * Time.deltaTime);
    }

    private void AvoidOtherCars()
    {
        // Check for nearby cars on both sides
        float avoidDistance = 1.5f; // Distance to check for other cars
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, -transform.right, avoidDistance);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, transform.right, avoidDistance);

        // Avoid left
        if (hitLeft.collider != null && hitLeft.collider.CompareTag("OtherCar"))
        {
            // Move to the right to avoid the car
            Vector3 avoidanceDirection = transform.right; // Move to the right
            transform.position += avoidanceDirection * speed * Time.deltaTime;
        }
        // Avoid right
        else if (hitRight.collider != null && hitRight.collider.CompareTag("OtherCar"))
        {
            // Move to the left to avoid the car
            Vector3 avoidanceDirection = -transform.right; // Move to the left
            transform.position += avoidanceDirection * speed * Time.deltaTime;
        }
    }

    private float GetLaneXPosition()
    {
        // Define your lane positions
        float[] lanePositions = { -1.33f, -0.43f, 0.5f, 1.43f };
        // Choose a random lane position
        int laneIndex = Random.Range(0, lanePositions.Length);
        return lanePositions[laneIndex];
    }


    private IEnumerator BlinkLights()
    {
        float blinkInterval = 0.2f;
        while (true) // Infinite loop
        {
            redLight.SetActive(true);
            blueLight.SetActive(false);
            yield return new WaitForSeconds(blinkInterval); // Wait

            redLight.SetActive(false);
            blueLight.SetActive(true);
            yield return new WaitForSeconds(blinkInterval); // Wait
        }
    }


}