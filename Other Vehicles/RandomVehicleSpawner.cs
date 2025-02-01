using UnityEngine;

public class RandomVehicleSpawner : MonoBehaviour
{
    public GameObject[] randomCarPrefab;
    public GameObject policeCarPrefab;
    public float spawnInterval = 2f;
    private Vector3[] lanePositions = new Vector3[4];

    private void Start()
    {
        lanePositions[0] = new Vector3(-1.33f, transform.position.y, transform.position.z);
        lanePositions[1] = new Vector3(-0.43f, transform.position.y, transform.position.z);
        lanePositions[2] = new Vector3(0.5f, transform.position.y, transform.position.z);
        lanePositions[3] = new Vector3(1.43f, transform.position.y, transform.position.z);

        StartCoroutine(SpawnRandomCars());

    }

    // Coroutine to spawn random cars at intervals
    private System.Collections.IEnumerator SpawnRandomCars()
    {
        while (true)
        {
            SpawnRandomCar();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // Method to spawn a random car
    private void SpawnRandomCar()
    {
        int randomIndex = Random.Range(0, randomCarPrefab.Length);
        // Instantiate a random car at a random x position in the lane
        float xPosition = lanePositions[Random.Range(0, lanePositions.Length)].x;
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0); // Adjust Y value as needed

        Instantiate(randomCarPrefab[randomIndex], spawnPosition, Quaternion.identity);
    }

}