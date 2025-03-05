using UnityEngine;

public class AchievementManager : MonoBehaviour
{

    [SerializeField] private GameObject achievementBarPrefab;

    private int maxAchievementCount = 2;

    void Start()
    {
        LoadAchievements();
    }

    private void LoadAchievements()
    {
        for (int i = 1; i <= maxAchievementCount; i++)
        {
            // Construct the config name
            string configName = $"ach_{i}";

            // Load each JSON file using the constructed name
            TextAsset jsonFile = Resources.Load<TextAsset>("AchievementsConfig/" + configName);
            if (jsonFile != null)
            {
                AchievementConfig achievementConfig = LoadAchievementConfig(jsonFile.text);
                GameObject achivementBar = Instantiate(achievementBarPrefab, transform);
                achivementBar.GetComponent<AchievementBar>().SetTaskDetails(achievementConfig);

            }
            else
            {
                Debug.LogWarning("JSON file not found for config name: " + configName);

            }
        }
    }
    private AchievementConfig LoadAchievementConfig(string json)
    {
        return JsonUtility.FromJson<AchievementConfig>(json);
    }
}