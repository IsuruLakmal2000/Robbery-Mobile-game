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
        policeSpawnInterval = GetPoliceSpawnInterval(levelNumber);
        if (levelNumber % 10 == 0)
        {
            //load boss levels from configs
            Debug.Log("Boss Level -----level no --" + levelNumber);
            LoadBossLevelConfig(levelNumber);
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
        // float basePoliceSpawnInterval = 8f;
        float baseOtherVehicleSpawnInterval = 8f;
        int baseRideExpences = 200;

        float levelMultiplier = Mathf.Max(1, levelNumber - 1);
        mapRotationSpeed = baseMapRotationSpeed + (0.5f * Mathf.Floor((levelNumber - 1) / 10));
        otherVehicleSpeed = baseOtherVehicleSpeed + (0.5f * Mathf.Floor((levelNumber - 1) / 10));
        robbedMoney = CalculateRobbedMoney(levelNumber);
        levelDistance = baseLevelDistance + (levelMultiplier * 3);
        levelBonusXp = Mathf.RoundToInt(baseLevelBonusXp + (levelMultiplier * 40));
        // policeSpawnInterval = Mathf.Max(2f, basePoliceSpawnInterval - (0.1f * levelMultiplier));
        // otherVehicleSpawnInterval = 1f;
        otherVehicleSpawnInterval = Mathf.Max(1f, baseOtherVehicleSpawnInterval - (0.1f * levelMultiplier));
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

    private float GetPoliceSpawnInterval(int levelNumber)
    {
        if (levelNumber >= 1 && levelNumber <= 5)
        {
            return 100f;
        }
        else if (levelNumber > 5 && levelNumber <= 10)
        {
            return 7f;

        }
        else if (levelNumber > 10 && levelNumber <= 15)
        {
            return 5f;
        }
        else if (levelNumber > 15 && levelNumber <= 20)
        {
            return 4f;
        }
        else if (levelNumber > 20 && levelNumber <= 30)
        {
            return 3f;
        }
        else if (levelNumber > 30 && levelNumber <= 50)
        {
            return 2.5f;
        }
        else
        {
            return 2f;
        }
    }
    int CalculateRobbedMoney(int level)
    {
        int baseRobbedMoney = 1000;
        return Mathf.RoundToInt(baseRobbedMoney * Mathf.Pow(growthFactor, level - 1));
    }

    void LoadBossLevelConfig(int levelNo)
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


//for information ------------- (based on above code how actually things change)
// Level	mapRotationSpeed Change	otherVehicleSpeed Change
// 1-10	Base Value	Base Value
// 11-20	+0.02	+0.05
// 21-30	+0.04	+0.10
// 31-40	+0.06	+0.15
// 41-50	+0.08	+0.20

// here above function illustrate looks like ---
// Illustration of Robbed Money per Level
// Level	Robbed Money ($)
// 1	1,000
// 10	3,707
// 20	13,742
// 30	50,957
// 40	188,951
// 50	700,743
// 60	2,598,105
// 70	9,635,831
// 80	35,739,218
// 90	132,538,942
// 100	200,624,132