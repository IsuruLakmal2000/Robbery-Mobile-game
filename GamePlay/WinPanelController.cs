using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class WinPanelController : MonoBehaviour
{

    [SerializeField] private Button nextBtn;
    [SerializeField] private TextMeshProUGUI robbedMoneyTxt;
    [SerializeField] private TextMeshProUGUI policeDestroyTxt;
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private GameObject vfxPrefabConfetti1;
    [SerializeField] private GameObject vfxPrefabConfetti2;
    public static WinPanelController instance;

    void Awake()
    {
        instance = this;
    }


    void Start()
    {
        nextBtn.onClick.AddListener(OnNextBtnClick);
        Instantiate(vfxPrefab, transform);
        Instantiate(vfxPrefabConfetti1, transform);
        Instantiate(vfxPrefabConfetti2, transform);

        policeDestroyTxt.text = "You destroyed " + LevelManager.instance.totalDestriyedPoliceVehiclesCount.ToString() + " police cars !";
    }

    private void OnNextBtnClick()
    {

        LevelUp();
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");

    }

    public void SetRobbedMoney(int money)
    {
        robbedMoneyTxt.text = money.ToString();
    }

    private void LevelUp()
    {

        int currentLevel = PlayerPrefs.GetInt("current_level", 1);
        int totalDestroyedPoliceCar = PlayerPrefs.GetInt("total_destroyed_police_car", 0);
        int totalMoneyInthisLevel = GameRobbedMoney.instance.robbedMoneyCount;
        PlayerPrefs.SetInt("total_money", PlayerPrefs.GetInt("total_money", 0) + totalMoneyInthisLevel);
        PlayerPrefs.SetInt("current_level", currentLevel + 1);
        PlayerPrefs.SetInt("total_destroyed_police_car", totalDestroyedPoliceCar + LevelManager.instance.totalDestriyedPoliceVehiclesCount);

    }
}