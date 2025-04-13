using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int baseSize; // Base size of the pool
    }

    public static ObjectPoolManager instance;

    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        InitializePools(PlayerPrefs.GetInt("current_level", 0)); // Initialize pools based on the current level
    }

    public void InitializePools(int level)
    {
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Calculate pool size based on the level
            int poolSize = CalculatePoolSize(pool.baseSize, level);

            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    private int CalculatePoolSize(int baseSize, int level)
    {
        // Example logic to scale pool size based on the level
        if (level < 10)
        {
            return baseSize;
        }
        else if (level < 20)
        {
            return baseSize + 3; // Increase pool size by 5 for levels 10-19
        }
        else if (level < 30)
        {
            return baseSize + 6; // Increase pool size by 10 for levels 20-29
        }
        else
        {
            return baseSize + 6; // Increase pool size by 15 for levels 30+
        }
    }

    public GameObject GetFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        GameObject obj = poolDictionary[tag].Dequeue();
        obj.SetActive(true);
        poolDictionary[tag].Enqueue(obj); // Re-enqueue the object for reuse
        return obj;
    }
}