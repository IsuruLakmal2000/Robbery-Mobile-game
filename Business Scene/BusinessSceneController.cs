using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BusinessSceneController : MonoBehaviour
{


    [SerializeField] private Button backBtn;


    void Start()
    {
        backBtn.onClick.AddListener(async () =>
        {
            int totalMoney = PlayerPrefs.GetInt("total_money", 0);
            await FirebaseController.instance.UpdateCurrentNetworth(PlayerPrefs.GetString("UserId"), totalMoney);
            SoundManager.instance.PlayButtonClick();
            SceneManager.LoadScene("Menu");
        });
    }
}