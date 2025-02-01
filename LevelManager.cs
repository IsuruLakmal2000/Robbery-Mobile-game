using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public float incrementPerSecond = 1f;
    public float totalDistance = 0f; // Total distance traveled
    public float levelTarget = 20f; // Initial target distance for level completion
    public float targetReductionRate = 1f; // Rate at which the target distance reduces
    public int robbedMoney = 0;
    public bool isLevelComplete = false;

    [SerializeField] private TextMeshProUGUI distanceToTravelTxt;

    void Start()
    {
        isLevelComplete = false;
        levelTarget = GameManager.instance.levelConfig.levelDistance;
        //  robbedMoney = GameManager.instance.levelConfig.robbedMoney;
    }

    void Update()
    {
        // Increment the total distance
        totalDistance += incrementPerSecond * Time.deltaTime;


        if (levelTarget <= 0)
        {
            if (isLevelComplete == false)
            {
                isLevelComplete = true;
                ShowWinPanel();
            }

            Debug.Log("level end");

            levelTarget = 0f;
            CarController.instance.CompleteLevel();
            GameManager.instance.isStoppingSpawnPolice = true;
            //   VictoryMenu();

        }
        else
        {
            levelTarget -= targetReductionRate * Time.deltaTime;
        }



    }

    private void OnGUI()
    {
        distanceToTravelTxt.text = Mathf.Round(levelTarget).ToString() + "m";
    }

    private void ShowWinPanel()
    {
        GamePlayPanelsController.instance.ShowWinPanel();
    }


}