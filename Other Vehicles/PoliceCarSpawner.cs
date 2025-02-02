using UnityEngine;

public class PoliceCarSpawner : MonoBehaviour
{
    public GameObject policeCarPrefab; // Reference to the police car prefab
    public Transform playerCar; // Reference to the player's car
    public float spawnDistance = 5f; // Distance behind the player car to spawn new police cars
    public float spawnInterval = 3f; // Time interval between police car spawns

    private void Start()
    {
        // Start the spawning coroutine

        StartCoroutine(SpawnPoliceCars());
        playerCar = GameObject.FindWithTag("PlayerCar").transform;

    }

    private System.Collections.IEnumerator SpawnPoliceCars()
    {
        while (!GameManager.instance.isStoppingSpawnPolice)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval);

            // Calculate the spawn position behind the player car
            //  Vector3 spawnPosition = playerCar.position + new Vector3(0, -spawnDistance, 0);
            Vector3 spawnPosition = transform.position;

            // Set the spawn position based on the player's lane
            spawnPosition.x = GetLaneXPosition(playerCar.position.x);

            // Instantiate the police car at the calculated position
            Instantiate(policeCarPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private float GetLaneXPosition(float playerX)
    {
        // Define your lane positions
        float[] lanePositions = { -1.33f, -0.43f, 0.5f, 1.43f };

        // Choose a lane based on the player's current x position
        float closestLane = lanePositions[0];
        float closestDistance = Mathf.Abs(playerX - closestLane);

        foreach (float lane in lanePositions)
        {
            float distance = Mathf.Abs(playerX - lane);
            if (distance < closestDistance)
            {
                closestLane = lane;
                closestDistance = distance;
            }
        }

        return closestLane; // Return the closest lane position
    }
}