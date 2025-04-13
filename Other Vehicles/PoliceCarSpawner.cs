// using UnityEngine;

// public class PoliceCarSpawner : MonoBehaviour
// {
//     public GameObject[] policeCarPrefab; // Reference to the police car prefab
//     private Transform playerCar;
//     public float spawnInterval = 1f; // Time interval between spawns
//     public bool isFrontSpawn = false; // Flag to determine spawn position

//     private void Start()
//     {
//         // Start the spawning coroutine
//         spawnInterval = GameManager.instance.policeSpawnInterval;
//         if (isFrontSpawn)
//         {
//             spawnInterval = spawnInterval * 3;
//         }

//         StartCoroutine(SpawnPoliceCars());
//         playerCar = GameObject.FindWithTag("PlayerCar").transform;
//         Debug.Log("police car spawn interval --" + spawnInterval * 2);
//     }

//     private System.Collections.IEnumerator SpawnPoliceCars()
//     {
//         while (!GameManager.instance.isStoppingSpawnPolice)
//         {
//             // Wait for the spawn interval

//             yield return new WaitForSeconds(spawnInterval * 2);

//             // Calculate the spawn position behind the player car
//             //  Vector3 spawnPosition = playerCar.position + new Vector3(0, -spawnDistance, 0);
//             Vector3 spawnPosition = transform.position;

//             // Set the spawn position based on the player's lane
//             if (playerCar != null)
//             {
//                 spawnPosition.x = GetLaneXPosition(playerCar.position.x);
//             }
//             else
//             {
//                 spawnPosition.x = 0;
//             }
//             int prefabIndex = GetPrefabIndexForLevel(GameManager.instance.levelNumber);
//             // Instantiate the police car at the calculated position
//             Instantiate(policeCarPrefab[prefabIndex], spawnPosition, Quaternion.identity);
//         }
//     }

//     private float GetLaneXPosition(float playerX)
//     {
//         // Define your lane positions
//         float[] lanePositions = { -1.33f, -0.43f, 0.5f, 1.43f };

//         // Choose a lane based on the player's current x position
//         float closestLane = lanePositions[0];
//         float closestDistance = Mathf.Abs(playerX - closestLane);

//         foreach (float lane in lanePositions)
//         {
//             float distance = Mathf.Abs(playerX - lane);
//             if (distance < closestDistance)
//             {
//                 closestLane = lane;
//                 closestDistance = distance;
//             }
//         }

//         return closestLane; // Return the closest lane position
//     }

//     private int GetPrefabIndexForLevel(int level)
//     {

//         if (level < 5)
//         {
//             return 0;
//         }
//         else if (level < 10)
//         {

//             int randomValue = Random.Range(0, 100);
//             if (randomValue < 70)
//             {
//                 return 0; // 75% chance for the first prefab
//             }
//             else
//             {
//                 return 1; // 25% chance for the second prefab
//             }
//         }
//         else if (level < 15)
//         {
//             return Random.Range(0, 2); // 50% chance for each prefab
//         }
//         else if (level < 20)
//         {
//             int randomValue = Random.Range(0, 100);
//             if (randomValue < 40)
//             {
//                 return 0;
//             }
//             else if (randomValue < 80 && randomValue >= 40)
//             {
//                 return 1;
//             }
//             else
//             {
//                 return 2;
//             }

//         }
//         else if (level < 30)
//         {
//             if (Random.Range(0, 100) < 70)
//             {
//                 return Random.Range(1, 3);
//             }
//             else
//             {
//                 return 0;
//             }
//         }
//         else if (level < 40)
//         {
//             if (Random.Range(0, 100) < 70)
//             {
//                 return Random.Range(2, 4);
//             }
//             else
//             {
//                 return Random.Range(0, 2);
//             }

//         }
//         else if (level < 50)
//         {
//             if (Random.Range(0, 100) < 70)
//             {
//                 return Random.Range(2, 4);
//             }
//             else
//             {
//                 return Random.Range(0, 2);
//             }

//         }
//         else
//         {
//             if (Random.Range(0, 100) < 75)
//             {
//                 return Random.Range(2, 4);
//             }
//             else
//             {
//                 return Random.Range(0, 2);
//             }
//         }
//     }
// }

using UnityEngine;

public class PoliceCarSpawner : MonoBehaviour
{
    public string[] policeCarTags; // Tags for different police car types
    private Transform playerCar;
    public float spawnInterval = 1f; // Time interval between spawns
    public bool isFrontSpawn = false; // Flag to determine spawn position

    private void Start()
    {
        // Start the spawning coroutine
        spawnInterval = GameManager.instance.policeSpawnInterval;
        if (isFrontSpawn)
        {
            spawnInterval = spawnInterval * 3;
        }

        StartCoroutine(SpawnPoliceCars());
        playerCar = GameObject.FindWithTag("PlayerCar").transform;
        Debug.Log("police car spawn interval --" + spawnInterval * 2);
    }

    private System.Collections.IEnumerator SpawnPoliceCars()
    {
        while (!GameManager.instance.isStoppingSpawnPolice)
        {
            // Wait for the spawn interval
            yield return new WaitForSeconds(spawnInterval * 2);

            // Calculate the spawn position behind the player car
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

            // Get the appropriate police car tag for the current level
            string policeCarTag = GetPoliceCarTagForLevel(GameManager.instance.levelNumber);

            // Get a police car from the object pool
            GameObject policeCar = ObjectPoolManager.instance.GetFromPool(policeCarTag);
            if (policeCar != null)
            {
                policeCar.transform.position = spawnPosition;
                policeCar.transform.rotation = Quaternion.identity;

                // Reset the police car's state
                PoliceCarController controller = policeCar.GetComponent<PoliceCarController>();
                if (controller != null)
                {
                    controller.ResetPoliceCar();
                }
            }
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

    private string GetPoliceCarTagForLevel(int level)
    {
        if (level < 5)
        {
            return policeCarTags[0];
        }
        else if (level < 10)
        {
            int randomValue = Random.Range(0, 100);
            return randomValue < 70 ? policeCarTags[0] : policeCarTags[1];
        }
        else if (level < 15)
        {
            return policeCarTags[Random.Range(0, 2)];
        }
        else if (level < 20)
        {
            int randomValue = Random.Range(0, 100);
            if (randomValue < 40) return policeCarTags[0];
            if (randomValue < 80) return policeCarTags[1];
            return policeCarTags[2];
        }
        else if (level < 30)
        {
            return Random.Range(0, 100) < 70 ? policeCarTags[Random.Range(1, 3)] : policeCarTags[0];
        }
        else if (level < 40)
        {
            return Random.Range(0, 100) < 70 ? policeCarTags[Random.Range(2, 4)] : policeCarTags[Random.Range(0, 2)];
        }
        else
        {
            return Random.Range(0, 100) < 75 ? policeCarTags[Random.Range(2, 4)] : policeCarTags[Random.Range(0, 2)];
        }
    }
}