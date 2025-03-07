using UnityEngine;

public class PoliceCarSpawner : MonoBehaviour
{
    public GameObject[] policeCarPrefab; // Reference to the police car prefab
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
            int prefabIndex = GetPrefabIndexForLevel(GameManager.instance.levelNumber);
            // Instantiate the police car at the calculated position
            Instantiate(policeCarPrefab[prefabIndex], spawnPosition, Quaternion.identity);
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

    private int GetPrefabIndexForLevel(int level)
    {

        if (level < 5)
        {
            return 0;
        }
        else if (level < 10)
        {

            int randomValue = Random.Range(0, 100);
            if (randomValue < 70)
            {
                return 0; // 75% chance for the first prefab
            }
            else
            {
                return 1; // 25% chance for the second prefab
            }
        }
        else if (level < 15)
        {
            return Random.Range(0, 2); // 50% chance for each prefab
        }
        else if (level < 20)
        {
            int randomValue = Random.Range(0, 100);
            if (randomValue < 40)
            {
                return 0;
            }
            else if (randomValue < 80 && randomValue >= 40)
            {
                return 1;
            }
            else
            {
                return 2;
            }

        }
        else if (level < 30)
        {
            if (Random.Range(0, 100) < 70)
            {
                return Random.Range(1, 3);
            }
            else
            {
                return 0;
            }
        }
        else if (level < 40)
        {
            if (Random.Range(0, 100) < 70)
            {
                return Random.Range(2, 4);
            }
            else
            {
                return Random.Range(0, 2);
            }

        }
        else if (level < 50)
        {
            if (Random.Range(0, 100) < 70)
            {
                return Random.Range(2, 4);
            }
            else
            {
                return Random.Range(0, 2);
            }

        }
        else
        {
            if (Random.Range(0, 100) < 75)
            {
                return Random.Range(2, 4);
            }
            else
            {
                return Random.Range(0, 2);
            }
        }
    }
}