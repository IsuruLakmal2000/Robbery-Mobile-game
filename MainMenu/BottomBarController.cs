using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
public class BottomBarController : MonoBehaviour
{
    private Button startBtn;
    private Button garageBtn;
    private Button shopBtn;
    private Button businessBtn;
    private Button leaderboardBtn;
    private Animator animator;
    [SerializeField] private Canvas canvas;
    private bool isPanelVisible = true;
    //[SerializeField] private GameObject vehiclePanelPrefab;
    [SerializeField] private GameObject backBtnOnGarage;
    [SerializeField] private GameObject garagePropSidePanelPrefab;
    [SerializeField] private GameObject leftSidePanelPrefab;
    [SerializeField] private GameObject shopPanelPrefab;
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject leaderboardPanelPrefab;

    public static BottomBarController instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        loadingPanel.SetActive(false);
        animator = GetComponent<Animator>();
        backBtnOnGarage.SetActive(false);
        startBtn = transform.Find("Start Btn").GetComponent<Button>();
        businessBtn = transform.Find("Business Btn").GetComponent<Button>();
        shopBtn = transform.Find("Shop Btn").GetComponent<Button>();
        garageBtn = transform.Find("Garage Btn").GetComponent<Button>();
        leaderboardBtn = transform.Find("Leaderboard Btn").GetComponent<Button>();
        leaderboardBtn.onClick.AddListener(OnLeaderboardBtnClick);
        startBtn.onClick.AddListener(OnStartBtnClick);
        shopBtn.onClick.AddListener(OnShopBtnClick);
        garageBtn.onClick.AddListener(OnGarageBtnClick);
        businessBtn.onClick.AddListener(OnBusinessBtnClick);
        backBtnOnGarage.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(BackButtonPressedCoroutine()));
    }

    private void OnStartBtnClick()
    {
        SoundManager.instance.PlayButtonClick();
        Debug.Log("on start clicked ");

        StartCoroutine(LoadSceneAsync("GameLevel"));
    }

    private void OnBusinessBtnClick()
    {
        SoundManager.instance.PlayButtonClick();
        Debug.Log("on business clicked ");
        StartCoroutine(LoadSceneAsync("Business"));

    }
    private void OnGarageBtnClick()
    {

        SoundManager.instance.PlayButtonClick();
        backBtnOnGarage.SetActive(true);
        Debug.Log("on garage clicked ");
        garagePropSidePanelPrefab.GetComponent<GarageSidePanelController>().TogglePanel();
        TogglePanel();
        VehiclePanelController.instance.TogglePanel();

        // vehiclePanelPrefab.GetComponent<VehiclePanelController>().TogglePanel();


    }
    public void OnShopBtnClick()
    {
        SoundManager.instance.PlayButtonClick();
        GameObject shopPanelInstance = Instantiate(shopPanelPrefab, canvas.transform);
        shopPanelInstance.transform.SetSiblingIndex(4);
        shopPanelInstance.transform.Find("Back Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            Destroy(shopPanelInstance);
        });
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingPanel.SetActive(true);
        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Optionally, you can display a loading screen or progress bar here
        Debug.Log("loading scene");

        while (!asyncLoad.isDone)
        {
            // You can also use asyncLoad.progress to show loading progress
            yield return null;
        }
    }

    public void TogglePanel()
    {
        if (isPanelVisible)
        {
            animator.SetBool("bottomBarOpen", false);
            leftSidePanelPrefab.SetActive(false);
        }
        else
        {
            animator.SetBool("bottomBarOpen", true);
            leftSidePanelPrefab.SetActive(true);
        }
        isPanelVisible = !isPanelVisible;
    }

    private IEnumerator BackButtonPressedCoroutine()
    {
        AdManager.instance.ShowInterstitialAd();
        int totalMoney = PlayerPrefs.GetInt("total_money", 0);
        PlayerPrefs.SetInt("total_networth", totalMoney);
        SoundManager.instance.PlayButtonClick();
        garagePropSidePanelPrefab.GetComponent<GarageSidePanelController>().TogglePanel();
        if (garagePropSidePanelPrefab.GetComponent<GarageSidePanelController>().healthBarInstance != null)
        {
            Destroy(garagePropSidePanelPrefab.GetComponent<GarageSidePanelController>().healthBarInstance);
        }
        VehiclePanelController.instance.ClosePanel();
        UpgradePanelController.instance.ClosePanel();
        GunsPanelController.instance.ClosePanel();
        //vehiclePanelPrefab.GetComponent<VehiclePanelController>().TogglePanel();
        TogglePanel();
        backBtnOnGarage.SetActive(false);
        yield break;
    }

    private void OnLeaderboardBtnClick()
    {
        SoundManager.instance.PlayButtonClick();
        Debug.Log("Leaderboard button clicked");
        // Assuming you have a prefab for the leaderboard panel
        GameObject leaderboardPanelInstance = Instantiate(leaderboardPanelPrefab, canvas.transform);
        leaderboardPanelInstance.transform.SetAsLastSibling();
        leaderboardPanelInstance.transform.Find("Back Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            Destroy(leaderboardPanelInstance);
        });
    }

}