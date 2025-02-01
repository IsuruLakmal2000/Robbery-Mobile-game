using System.Collections;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public float speed = 2f; // Speed of the random car
    private Vector3 targetPosition;

    // Lane positions
    private Vector3[] lanePositions = new Vector3[4];

    private int collisionCount = 0; // To track collisions
    public float moveDuration = 0.5f;
    private Rigidbody2D rb;


    public static CoinBehaviour instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
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
        // Set the increased speed after collision
    }

    private void Update()
    {
        MoveCoin();
    }

    private void MoveCoin()
    {
        transform.position += Vector3.down * speed * Time.deltaTime; // Move the vehicle downward
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCar")) // Ensure your player car has the tag "Player"
        {
            StartCoroutine(MoveAndDestroy());

        }
        if (collision.gameObject.CompareTag("Area"))
        {
            // Destroy the coin
            Destroy(gameObject);
        }

    }




    private IEnumerator MoveAndDestroy()
    {
        // Instantiate the collection effect if needed


        Vector3 startPosition = transform.position;
        Vector3 endPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        endPosition.z = 0; // Set z to 0 for 2D

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / moveDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure it ends at the target position
        transform.position = endPosition;

        // Destroy the coin
        Destroy(gameObject);
    }
}