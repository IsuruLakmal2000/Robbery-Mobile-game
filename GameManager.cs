using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int totalCoinCount = 0;
    public LevelProperties levelConfig;
    public bool isStoppingSpawnPolice = false;
    public static GameManager instance;
    // for normal values 
    //--------------------------------------------------------------------------------------
    private int levelNumber;
    public float mapRotationSpeed;
    public float levelDistance;
    public int robbedMoney;
    public float otherVehicleSpeed;
    public int levelBonusXp;
    public float policeSpawnInterval;
    public float otherVehicleSpawnInterval;
    public int rideExpences;
    private float growthFactor = 1.12f;


    /// -----------------------------------------------------------------------------



    private void Awake()
    {
        instance = this;
        totalCoinCount = PlayerPrefs.GetInt("total_coin_count", 0);
        levelNumber = PlayerPrefs.GetInt("current_level", 1);
        if (levelNumber % 10 == 0)
        {
            //load boss levels from configs
            Debug.Log("Boss Level -----level no --" + levelNumber);
            LoadLevelConfig(levelNumber);
        }
        else
        {
            //load normal levels
            Debug.Log("Normal Level -----level no --" + levelNumber);
            LoadNormalLevels();
        }

    }

    void Start()
    {

        //BackgroundController.instance.speed = levelConfig.mapRotationSpeed;
    }

    private void LoadNormalLevels()
    {
        float baseMapRotationSpeed = 0.5f;
        float baseLevelDistance = 30f;
        float baseOtherVehicleSpeed = 2f;
        int baseLevelBonusXp = 200;
        float basePoliceSpawnInterval = 5f;
        float baseOtherVehicleSpawnInterval = 8f;
        int baseRideExpences = 200;

        float levelMultiplier = Mathf.Max(1, levelNumber - 1);
        mapRotationSpeed = baseMapRotationSpeed + (0.02f * Mathf.Floor((levelNumber - 1) / 10));
        otherVehicleSpeed = baseOtherVehicleSpeed + (0.05f * Mathf.Floor((levelNumber - 1) / 10));
        robbedMoney = CalculateRobbedMoney(levelNumber);
        levelDistance = baseLevelDistance + (levelMultiplier * 3);
        levelBonusXp = Mathf.RoundToInt(baseLevelBonusXp + (levelMultiplier * 40));
        policeSpawnInterval = Mathf.Max(2f, basePoliceSpawnInterval - (0.1f * levelMultiplier));
        otherVehicleSpawnInterval = Mathf.Max(4f, baseOtherVehicleSpawnInterval - (0.1f * levelMultiplier));
        rideExpences = Mathf.RoundToInt(baseRideExpences + (levelMultiplier * 40));
        // -asign those values in to leevl prop class

        levelConfig.levelNumber = levelNumber;
        levelConfig.mapRotationSpeed = mapRotationSpeed;
        levelConfig.levelDistance = levelDistance;
        levelConfig.robbedMoney = robbedMoney;
        levelConfig.otherVehicleSpeed = otherVehicleSpeed;
        levelConfig.levelBonusXp = levelBonusXp;
        levelConfig.otherVehicleSpawnInterval = otherVehicleSpawnInterval;
        levelConfig.rideExpences = rideExpences;
    }

    int CalculateRobbedMoney(int level)
    {
        int baseRobbedMoney = 1000;
        return Mathf.RoundToInt(baseRobbedMoney * Mathf.Pow(growthFactor, level - 1));
    }

    void LoadLevelConfig(int levelNo)
    {
        // Load JSON file from Resources folder

        string fileName = $"levelConfigs/Level{levelNo}";
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

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