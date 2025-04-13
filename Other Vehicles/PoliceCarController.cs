using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PoliceCarController : MonoBehaviour
{
    private bool isMovingUncontrollably = false;
    private Quaternion originalRotation;
    public Transform playerCar;
    public float followDistance = 2f;
    public float speed = 4f;
    public float hitDistance = 0.5f;
    public float resetDistance = 5f;
    public float passDistance = 1.5f;
    public float collisionChance = 0.3f;
    public float laneChangeChance = 0.05f;
    public float laneWidth = 1f;
    private float targetLaneX;
    public Slider healthBar;
    public float maxHealth = 10f;
    private float currentHealth;
    private bool isWalkieTalkieSoundPlayed = false;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private int destroyEarning = 500;

    [SerializeField] private GameObject exPlosionPrefab;
    [SerializeField] private GameObject redLight;
    [SerializeField] private GameObject blueLight;
    [SerializeField] private GameObject moneyBoomExplosionPrefab;

    [SerializeField] private AudioSource policeCarAudioSource;
    private float targetVolume = 0f;
    [SerializeField] private AudioClip sirenSound;
    [SerializeField] private AudioClip walkietalkiesound;

    public float volumeChangeSpeed = 0.2f;
    private float maxDistance = 5f;
    private float minDistance = 0.1f;

    [SerializeField] private Canvas canvas;

    private void Start()
    {
        originalRotation = transform.rotation;
        StartCoroutine(BlinkLights());
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
        policeCarAudioSource.clip = sirenSound;
        policeCarAudioSource.volume = 0f;
        policeCarAudioSource.loop = true;
        policeCarAudioSource.Play();

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
        }
        else
        {
            PoliceCarSirenController();
            FollowPlayer();
        }
        // Smoothly rotate back to the original rotation if needed
        if (Quaternion.Angle(transform.rotation, originalRotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, 200 * Time.deltaTime);
        }
    }


    private void PlayWalkieTalkieSound()
    {
        policeCarAudioSource.volume = 0.3f;
        policeCarAudioSource.PlayOneShot(walkietalkiesound);
    }
    private void PoliceCarSirenController()
    {
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(playerCar.position.x, playerCar.position.y));

        if (distance < 1f && !isWalkieTalkieSoundPlayed)
        {
            PlayWalkieTalkieSound();
            isWalkieTalkieSoundPlayed = true;
        }
        // Determine the target volume based on distance
        if (distance < 0.1f)
        {
            targetVolume = 0.3f; // Full volume
        }
        else if (distance > maxDistance)
        {
            Debug.Log("Police car far from player: " + distance);
            targetVolume = 0f; // No sound
        }
        else
        {
            targetVolume = 0.3f * (1 - (distance - minDistance) / (maxDistance - minDistance));
        }
        policeCarAudioSource.volume = Mathf.Lerp(policeCarAudioSource.volume, targetVolume, Time.deltaTime * volumeChangeSpeed);
    }

    private void FollowPlayer()
    {

        Vector3 targetPosition = new Vector3(playerCar.position.x, playerCar.position.y - followDistance, playerCar.position.z);
        targetPosition.x = targetLaneX; // Ensure it stays in the target lane

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
            // StartCoroutine(UncontrollableMovement(collision));
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
    private IEnumerator UncontrollableMovement(Collision2D collision)
    {
        // Check the contact point to determine the side of the hit
        Vector2 contactPoint = collision.contacts[0].point;
        Vector2 policeCarPosition = transform.position;

        // Set the uncontrollable movement flag
        isMovingUncontrollably = true;

        // Get the Rigidbody2D component
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            yield break; // Exit if no Rigidbody2D is attached
        }

        // Apply an initial velocity based on the collision impact
        Vector2 impactDirection = (policeCarPosition - contactPoint).normalized;
        rb.linearVelocity = impactDirection * speed;

        // Randomly rotate the vehicle upon collision
        float randomTorque = Random.Range(-50f, 50f);
        rb.AddTorque(randomTorque);

        // Uncontrollable movement for 2 seconds
        float duration = 2f;
        float timer = 0f;

        while (timer < duration)
        {
            // Gradually reduce the velocity to simulate friction
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, timer / duration);

            timer += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // After 2 seconds, reset the flag and stop the vehicle
        isMovingUncontrollably = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
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
            SoundManager.instance.PlayVehicleDestroyedSound();
            // Destroy(moneyIncreaseVfx, 1f);
            Destroy(explosion, 1f);
            // Time.timeScale = 0f;
            GameRobbedMoney.instance.IncreaseMoneyWhenCollect(destroyEarning);
            gameObject.SetActive(false);

        }
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

    public void ResetPoliceCar()
    {
        // Reset health
        currentHealth = maxHealth;
        healthBar.value = currentHealth;

        // Reset position and rotation
        //  transform.position = Vector3.zero;
        // transform.rotation = Quaternion.identity;

        // Reset any other properties
        isMovingUncontrollably = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        GetComponent<Rigidbody2D>().angularVelocity = 0f;
    }
}