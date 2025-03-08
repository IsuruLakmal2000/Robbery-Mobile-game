using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coinPrefab;
    public GameObject gemPrefab;
    public GameObject adMoneyPrefab;
    public float spawnInterval = 1f;

    private Vector3[] lanePositions = new Vector3[4];
    private bool gemSpawned = false;

    private void Start()
    {
        lanePositions[0] = new Vector3(-1.33f, transform.position.y, transform.position.z);
        lanePositions[1] = new Vector3(-0.43f, transform.position.y, transform.position.z);
        lanePositions[2] = new Vector3(0.5f, transform.position.y, transform.position.z);
        lanePositions[3] = new Vector3(1.43f, transform.position.y, transform.position.z);

        StartCoroutine(SpawnCoins());
    }

    private System.Collections.IEnumerator SpawnCoins()
    {
        float elapsedTime = 0f; // Track elapsed time
        while (true)
        {
            if (elapsedTime % 20 == 0)
            {
                SpawnGem();
                //gemSpawned = true;
            }
            else if (elapsedTime == 10 || elapsedTime == 30 || elapsedTime == 55 || elapsedTime == 80 || elapsedTime == 100)
            {
                SpawnAdMoney();
            }
            else
            {
                SpawnCoin();
                yield return new WaitForSeconds(spawnInterval);
                elapsedTime += spawnInterval;
            }
        }
    }

    private void SpawnCoin()
    {
        int randomIndex = Random.Range(0, coinPrefab.Length);
        float xPosition = lanePositions[Random.Range(0, lanePositions.Length)].x;
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        Instantiate(coinPrefab[randomIndex], spawnPosition, Quaternion.identity);
    }

    private void SpawnAdMoney()
    {
        float xPosition = lanePositions[Random.Range(0, lanePositions.Length)].x;
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        Instantiate(adMoneyPrefab, spawnPosition, Quaternion.identity);
    }

    private void SpawnGem()
    {
        float xPosition = lanePositions[Random.Range(0, lanePositions.Length)].x;
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        Instantiate(gemPrefab, spawnPosition, Quaternion.identity);
    }
}