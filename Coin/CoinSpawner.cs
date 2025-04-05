using TMPro;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coinPrefab;
    public GameObject gemPrefab;
    public GameObject adMoneyPrefab;
    public float spawnInterval = 1f;

    private Vector3[] lanePositions = new Vector3[4];
    private bool gemSpawned = false;
    private float currentWatchAddPrice = 0;
    private void Start()
    {
        lanePositions[0] = new Vector3(-1.33f, transform.position.y, transform.position.z);
        lanePositions[1] = new Vector3(-0.43f, transform.position.y, transform.position.z);
        lanePositions[2] = new Vector3(0.5f, transform.position.y, transform.position.z);
        lanePositions[3] = new Vector3(1.43f, transform.position.y, transform.position.z);
        currentWatchAddPrice = GameManager.instance.levelNumber * 1000;
        StartCoroutine(SpawnCoins());
    }

    private System.Collections.IEnumerator SpawnCoins()
    {
        float elapsedTime = 0f; // Track elapsed time
        while (true)
        {
            if (elapsedTime % 20 == 0 && elapsedTime > 1)
            {
                SpawnGem();
                yield return new WaitForSeconds(spawnInterval);
                elapsedTime += spawnInterval;
                //gemSpawned = true;
            }
            else if (elapsedTime == 5 || elapsedTime == 10 || elapsedTime == 90 || elapsedTime == 120)
            {
                SpawnAdMoney();
                yield return new WaitForSeconds(spawnInterval);
                elapsedTime += spawnInterval;
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
        //float currentWatchAddPrice = PlayerPrefs.GetFloat("watch_ads_current_price", 1000);
        GameObject admoneyInstance = Instantiate(adMoneyPrefab, spawnPosition, Quaternion.identity);
        admoneyInstance.transform.Find("Canvas/rewardCount").GetComponent<TextMeshProUGUI>().text = FormatPrice(currentWatchAddPrice).ToString();
        currentWatchAddPrice = currentWatchAddPrice * 1.2f;


    }

    private void SpawnGem()
    {
        float xPosition = lanePositions[Random.Range(0, lanePositions.Length)].x;
        Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, 0);

        Instantiate(gemPrefab, spawnPosition, Quaternion.identity);
    }

    private string FormatPrice(float price)
    {
        if (price >= 1000000) // 1M and above
            return (price / 1000000f).ToString("0.##") + "M";
        else if (price >= 1000) // 1K and above
            return (price / 1000f).ToString("0.##") + "K";
        else
            return price.ToString(); // If less than 1K, show as is
    }
}