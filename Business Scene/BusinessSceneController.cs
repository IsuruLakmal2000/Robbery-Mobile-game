using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BusinessSceneController : MonoBehaviour
{
    [SerializeField] private Button backBtn;
    [SerializeField] private GameObject loadingPanel; // Reference to the loading text UI element

    void Start()
    {
        loadingPanel.gameObject.SetActive(false); // Ensure the loading text is hidden initially

        backBtn.onClick.AddListener(async () =>
        {
            AdManager.instance.ShowInterstitialAd();
            loadingPanel.gameObject.SetActive(true); // Show the loading text
            int totalMoney = PlayerPrefs.GetInt("total_money", 0);
            PlayerPrefs.SetInt("total_networth", totalMoney);
            SoundManager.instance.PlayButtonClick();
            await LoadMenuSceneAsync(); // Load the scene asynchronously
        });
    }

    private async System.Threading.Tasks.Task LoadMenuSceneAsync()
    {
        var asyncOperation = SceneManager.LoadSceneAsync("Menu");
        while (!asyncOperation.isDone)
        {
            // Optionally, you can update a progress bar here using asyncOperation.progress
            await System.Threading.Tasks.Task.Yield(); // Wait for the next frame
        }
    }
}