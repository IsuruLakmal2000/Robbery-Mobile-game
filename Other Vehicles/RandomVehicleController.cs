using UnityEngine;

public class RandomVehicleController : MonoBehaviour
{
    public float speed = 2f; // Speed of the random car
    private Vector3 targetPosition;

    // Lane positions
    private Vector3[] lanePositions = new Vector3[4];

    private int collisionCount = 0; // To track collisions
    private bool isMovingUncontrollably = false;
    private float increasedSpeed; // Speed after collision
    private float originalSpeed; // To store original speed
    private float recoveryTime = 1f; // Time to return to original speed
    private float moveAwayDuration = 0.5f; // Duration to move away after collision
    private float moveAwayStartTime;
    private Rigidbody2D rb;

    private void Start()
    {
        speed = GameManager.instance.levelConfig.otherVehicleSpeed;
        lanePositions[0] = new Vector3(-1.33f, transform.position.y, transform.position.z);
        lanePositions[1] = new Vector3(-0.43f, transform.position.y, transform.position.z);
        lanePositions[2] = new Vector3(0.5f, transform.position.y, transform.position.z);
        lanePositions[3] = new Vector3(1.43f, transform.position.y, transform.position.z);

        // Choose a random lane
        int randomLane = Random.Range(0, lanePositions.Length);
        transform.position = lanePositions[randomLane];

        // Set target position (moving down the road)
        targetPosition = new Vector3(transform.position.x, transform.position.y - 13f, transform.position.z); // Adjust Y value as needed
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = speed;
        increasedSpeed = speed * 1.3f; // Set the increased speed after collision
    }

    private void Update()
    {
        MoveCar();

    }

    private void MoveCar()
    {
        if (isMovingUncontrollably)
        {
            // Calculate the movement direction based on the vehicle's rotation
            Vector3 moveDirection = transform.up; // Get the vehicle's forward direction
            moveDirection.y = -1; // Ensure it always moves downward

            // Normalize the direction to maintain speed
            moveDirection.Normalize();
            transform.position += moveDirection * speed * Time.deltaTime; // Move the vehicle in the determined direction
        }
        else
        {
            // Move straight down when not in an uncontrollable state
            transform.position += Vector3.down * speed * Time.deltaTime; // Move the vehicle downward
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCar"))
        {
            HandleCollisionWithPlayer(collision);
        }

        if (collision.gameObject.CompareTag("Area"))
        {
            Destroy(gameObject);
        }
    }

    private void HandleCollisionWithPlayer(Collision2D collision)
    {
        collisionCount++;
        Debug.Log($"Collision detected with player! Collision count: {collisionCount}");

        // Randomly rotate the vehicle upon collision
        // float randomAngle = Random.Range(-30f, 30f); // Adjust the range as needed
        // transform.Rotate(0, 0, randomAngle); // Rotate the vehicle

        // Set the vehicle to move uncontrollably
        isMovingUncontrollably = true; // Indicate that the vehicle is now moving in a new direction
    }
}