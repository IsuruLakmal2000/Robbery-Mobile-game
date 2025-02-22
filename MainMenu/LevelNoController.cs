using UnityEngine;
using TMPro;
public class LevelNoController : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI levelNoTxt;

    void Start()
    {
        levelNoTxt.text = PlayerPrefs.GetInt("current_level", 1).ToString();
    }


    private void OnGUI()
    {
        levelNoTxt.text = PlayerPrefs.GetInt("current_level", 1).ToString();
    }
}