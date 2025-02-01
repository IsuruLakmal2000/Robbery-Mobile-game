using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalCoinCount = 0;
    public LevelProperties levelConfig;
    public bool isStoppingSpawnPolice = false;
    public static GameManager instance;



    private void Awake()
    {
        instance = this;
        totalCoinCount = PlayerPrefs.GetInt("total_coin_count", 0);
        LoadLevelConfig();
    }

    void Start()
    {

        //BackgroundController.instance.speed = levelConfig.mapRotationSpeed;
    }


    void LoadLevelConfig()
    {
        // Load JSON file from Resources folder
        TextAsset jsonFile = Resources.Load<TextAsset>("levelConfigs/Level1");

        if (jsonFile != null)
        {
            // Deserialize JSON to LevelConfig object
            levelConfig = JsonUtility.FromJson<LevelProperties>(jsonFile.text);
            Debug.Log("Level Config Loaded Successfully!");
        }
        else
        {
            Debug.LogError("Failed to load level configuration file!");
        }
    }
}