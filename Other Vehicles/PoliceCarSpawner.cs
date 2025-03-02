using UnityEngine;

public class PoliceCarSpawner : MonoBehaviour
{
    public GameObject policeCarPrefab; // Reference to the police car prefab
    private Transform playerCar;

    private void Start()
    {
        // Start the spawning coroutine

        StartCoroutine(SpawnPoliceCars());
        playerCar = GameObject.FindWithTag("PlayerCar").transform;
        Debug.Log("police car spawn interval --" + GameManager.instance.policeSpawnInterval);
    }

    private System.Collections.IEnumerator SpawnPoliceCars()
    {
        while (!GameManager.instance.isStoppingSpawnPolice)
        {
            // Wait for the spawn interval

            yield return new WaitForSeconds(GameManager.instance.policeSpawnInterval);

            // Calculate the spawn position behind the player car
            //  Vector3 spawnPosition = playerCar.position + new Vector3(0, -spawnDistance, 0);
            Vector3 spawnPosition = transform.position;

            // Set the spawn position based on the player's lane
            if (playerCar != null)
            {
                spawnPosition.x = GetLaneXPosition(playerCar.position.x);
            }
            else
            {
                spawnPosition.x = 0;
            }

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