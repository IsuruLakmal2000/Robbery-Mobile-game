using UnityEngine;
using System;

public class XPSystem : MonoBehaviour
{
    public static XPSystem Instance;

    public int currentLevel;
    public int currentXP;
    public int xpToNextLevel;

    public event Action<int> OnLevelUp;

    private void Awake()
    {
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
        }

        SaveXPData();
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel; // Carry over extra XP
        currentLevel++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f); // Increase next level XP

        Debug.Log($"🎉 Level Up! New Level: {currentLevel} | Next XP Target: {xpToNextLevel}");

        OnLevelUp?.Invoke(currentLevel); // Update UI
        SaveXPData(); // Save XP & Level
    }

    private void SaveXPData()
    {
        PlayerPrefs.SetInt("XP_Level", currentLevel);
        PlayerPrefs.SetInt("XP_Amount", currentXP);
        PlayerPrefs.SetInt("XP_NextLevel", xpToNextLevel);
        PlayerPrefs.Save();
        Debug.Log("XP Data Saved!");
    }

    private void LoadXPData()
    {
        currentLevel = PlayerPrefs.GetInt("XP_Level", 1);
        currentXP = PlayerPrefs.GetInt("XP_Amount", 0);
        xpToNextLevel = PlayerPrefs.GetInt("XP_NextLevel", 1000);
        Debug.Log($"XP Data Loaded: Level {currentLevel}, XP {currentXP}/{xpToNextLevel}");
    }
}
