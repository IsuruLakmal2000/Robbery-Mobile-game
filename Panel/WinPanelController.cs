using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class WinPanelController : MonoBehaviour
{

    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI robbedMoneyTxt;
    [SerializeField] private TextMeshProUGUI collectedGemCountTxt;
    [SerializeField] private TextMeshProUGUI policeDestroyTxt;
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private GameObject vfxPrefabConfetti1;
    [SerializeField] private GameObject vfxPrefabConfetti2;
    [SerializeField] private TextMeshProUGUI earnedXpTxt;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    private Coroutine xpAnimationCoroutine;
    private int displayedXP = 0;
    public static WinPanelController instance;

    void Awake()
    {
        instance = this;
    }

    void PauseGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Paused");
    }
    void Start()
    {

        nextBtn.onClick.AddListener(OnNextBtnClick);
        Instantiate(vfxPrefab, transform);
        Instantiate(vfxPrefabConfetti1, transform);
        Instantiate(vfxPrefabConfetti2, transform);
        int currentXpLevel = PlayerPrefs.GetInt("XP_Level", 1);
        currentLevelTxt.text = currentXpLevel.ToString();
        int totalEarnedXP = Mathf.RoundToInt((GameManager.instance.levelConfig.levelBonusXp +
                                             LevelManager.instance.totalDestriyedPoliceVehiclesCount * 50)
                                             * currentXpLevel * 0.2f + LevelManager.instance.totalDistance * 1.5f);

        PlayerPrefs.SetInt("PendingXP", totalEarnedXP);
        UpdateEarnedXP(totalEarnedXP);
        policeDestroyTxt.text = "You destroyed " + LevelManager.instance.totalDestriyedPoliceVehiclesCount.ToString() + " police cars !";
        Invoke("PauseGame", 0.5f);
    }

    private void OnNextBtnClick()
    {

        LevelUp();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");

    }

    public void SetRobbedMoney(int money, int gem)
    {
        robbedMoneyTxt.text = money.ToString();
        collectedGemCountTxt.text = gem.ToString();

    }

    private void LevelUp()
    {

        int currentLevel = PlayerPrefs.GetInt("current_level", 1);
        int totalDestroyedPoliceCar = PlayerPrefs.GetInt("total_destroyed_police_car", 0);
        int totalMoneyInthisLevel = GameRobbedMoney.instance.robbedMoneyCount;
        int totalGemsEarnedInthisLevel = GameRobbedMoney.instance.gameCollectedGemCount;
        PlayerPrefs.SetInt("total_gem", PlayerPrefs.GetInt("total_gem", 0) + totalGemsEarnedInthisLevel);
        PlayerPrefs.SetInt("total_money", PlayerPrefs.GetInt("total_money", 0) + totalMoneyInthisLevel);
        PlayerPrefs.SetInt("current_level", currentLevel + 1);
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
}