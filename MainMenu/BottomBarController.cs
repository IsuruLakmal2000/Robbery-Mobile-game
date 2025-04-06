using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;
public class BottomBarController : MonoBehaviour
{
    private Button startBtn;
    private Button leaderboardBtn;
    private Button garageBtn;
    private Button shopBtn;
    private Button businessBtn;
    private Animator animator;
    [SerializeField] private Canvas canvas;
    private bool isPanelVisible = true;
    //[SerializeField] private GameObject vehiclePanelPrefab;
    [SerializeField] private GameObject backBtnOnGarage;
    [SerializeField] private GameObject garagePropSidePanelPrefab;
    [SerializeField] private GameObject leftSidePanelPrefab;
    [SerializeField] private GameObject leaderboardPanelPrefab;

    void Start()
    {
        animator = GetComponent<Animator>();
        backBtnOnGarage.SetActive(false);
        startBtn = transform.Find("Start Btn").GetComponent<Button>();
        businessBtn = transform.Find("Business Btn").GetComponent<Button>();
        shopBtn = transform.Find("Shop Btn").GetComponent<Button>();
        leaderboardBtn = transform.Find("Leaderboard Btn").GetComponent<Button>();
        garageBtn = transform.Find("Garage Btn").GetComponent<Button>();
        startBtn.onClick.AddListener(OnStartBtnClick);
        leaderboardBtn.onClick.AddListener(OnLeaderboardBtnClick);
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
        SceneManager.LoadScene("Business");
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
    private void OnLeaderboardBtnClick()
    {
        SoundManager.instance.PlayButtonClick();
        GameObject leaderboardPanelInstance = Instantiate(leaderboardPanelPrefab, canvas.transform);
        leaderboardPanelInstance.transform.SetAsLastSibling();
        leaderboardPanelInstance.transform.Find("Back Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            Destroy(leaderboardPanelInstance);
        });
    }
    private IEnumerator LoadSceneAsync(string sceneName)
    {
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
        var updateTask = FirebaseController.instance.UpdateCurrentNetworth(PlayerPrefs.GetString("UserId"), totalMoney);
        while (!updateTask.IsCompleted)
        {
            yield return null;
        }
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
    }

}