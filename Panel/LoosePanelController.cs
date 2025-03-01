using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoosePanelController : MonoBehaviour
{
    [SerializeField] private Button nextBtn;
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private TextMeshProUGUI robbedMoneyTxt;
    [SerializeField] private TextMeshProUGUI policeDestroyTxt;
    [SerializeField] private TextMeshProUGUI earnedXpTxt;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    private Coroutine xpAnimationCoroutine;
    private int displayedXP = 0;
    public static LoosePanelController instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {

        nextBtn.onClick.AddListener(OnNextBtnClick);
        Instantiate(vfxPrefab, transform);

        currentLevelTxt.text = PlayerPrefs.GetInt("XP_Level", 1).ToString();
        int totalEarnedXP = Mathf.RoundToInt(LevelManager.instance.totalDestriyedPoliceVehiclesCount * 50 + LevelManager.instance.totalDistance * 1.5f);

        PlayerPrefs.SetInt("PendingXP", totalEarnedXP);
        UpdateEarnedXP(totalEarnedXP);
        policeDestroyTxt.text = "You destroyed " + LevelManager.instance.totalDestriyedPoliceVehiclesCount.ToString() + " police cars !";
        Invoke("PauseGame", 1f);
    }


    private void OnNextBtnClick()
    {

        LevelFailed();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }
    public void SetFine(int fine)
    {
        robbedMoneyTxt.text = "-" + fine.ToString();
    }
    private void LevelFailed()
    {
        int totalDestroyedPoliceCar = PlayerPrefs.GetInt("total_destroyed_police_car", 0);
        int fine = 1000;
        PlayerPrefs.SetInt("total_money", PlayerPrefs.GetInt("total_money", 0) - fine);
        PlayerPrefs.SetInt("total_destroyed_police_car", totalDestroyedPoliceCar + LevelManager.instance.totalDestriyedPoliceVehiclesCount);

    }
    public void UpdateEarnedXP(int totalEarnedXP)
    {
        // Stop any existing animation to avoid overlapping
        if (xpAnimationCoroutine != null)
        {
            StopCoroutine(xpAnimationCoroutine);
        }

        // Start new animation
        xpAnimationCoroutine = StartCoroutine(AnimateXPIncrease(displayedXP, totalEarnedXP, 1.0f)); // 1s duration
    }
    private IEnumerator AnimateXPIncrease(int startXP, int targetXP, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration; // Normalize 0 → 1 over time
            displayedXP = Mathf.RoundToInt(Mathf.Lerp(startXP, targetXP, t));
            earnedXpTxt.text = displayedXP.ToString();
            yield return null;
        }

        // Ensure final XP is correctly set
        displayedXP = targetXP;
        earnedXpTxt.text = displayedXP.ToString();
    }
    void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }
}