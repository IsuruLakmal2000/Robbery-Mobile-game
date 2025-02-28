using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class BusinessSceneController : MonoBehaviour
{


    [SerializeField] private Button backBtn;


    void Start()
    {
        backBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Menu");
        });
    }
}