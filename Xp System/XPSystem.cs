using UnityEngine;
using System;
using System.Threading.Tasks;

public class XPSystem : MonoBehaviour
{
    public static XPSystem Instance;

    public int currentLevel;
    public int currentXP;
    public int xpToNextLevel;
    [SerializeField] private GameObject levelUpPanel;
    private Canvas canvas;

    public event Action<int> OnLevelUp;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        if (Instance == null)
            Instance = this;

        LoadXPData();
    }
    void Start()
    {
        int pendingXP = PlayerPrefs.GetInt("PendingXP", 0);
        if (pendingXP > 0)
        {
            AddXP(pendingXP);
            PlayerPrefs.SetInt("PendingXP", 0); // Reset stored XP
            PlayerPrefs.Save();
        }
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        Debug.Log($"Gained {amount} XP. Total XP: {currentXP}");

        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
            ShowLevelUpPanel();
        }

        SaveXPData();
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel; // Carry over extra XP
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f); // Increase next level XP
        PlayerPrefs.SetInt("XP_Level", currentLevel);
        Debug.Log($"🎉 Level Up! New Level: {currentLevel} | Next XP Target: {xpToNextLevel}");

        OnLevelUp?.Invoke(currentLevel); // Update UI
        SaveXPData(); // Save XP & Level
    }

    private async Task SaveXPData()
    {
        // Save XP system level (XP_Level) and related data
        PlayerPrefs.SetInt("XP_Level", currentLevel); // Save XP system level
        PlayerPrefs.SetInt("XP_Amount", currentXP);
        PlayerPrefs.SetInt("XP_NextLevel", xpToNextLevel);

        PlayerPrefs.Save();
        Debug.Log("XP Data Saved!");
    }

    private void LoadXPData()
    {
        // Load XP system level (XP_Level) and related data
        currentLevel = PlayerPrefs.GetInt("XP_Level", 1); // Load XP system level
        currentXP = PlayerPrefs.GetInt("XP_Amount", 0);
        xpToNextLevel = PlayerPrefs.GetInt("XP_NextLevel", 200);

        Debug.Log($"XP Data Loaded: XP Level {currentLevel}, XP {currentXP}/{xpToNextLevel}");
    }
    private void ShowLevelUpPanel()
    {
        GameObject levelUpPanelInstance = Instantiate(levelUpPanel, canvas.transform);
        levelUpPanelInstance.transform.SetAsLastSibling();
        levelUpPanelInstance.GetComponent<LevelUpPanelController>().SetLevel(currentLevel);
    }
}
