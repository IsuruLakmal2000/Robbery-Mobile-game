using UnityEngine;

public class MainMenuPanelController : MonoBehaviour
{

    [SerializeField] private GameObject popupPanel;
    [SerializeField] private GameObject createAccountPanel;
    private Canvas canvas;

    public static MainMenuPanelController Instance;

    void Awake()
    {


        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


    }

    void Start()
    {
        canvas = FindFirstObjectByType<Canvas>();
        if (PlayerPrefs.GetInt("IsFirstTime", 1) == 1)
        {

            GameObject createAccInstance = Instantiate(createAccountPanel, canvas.transform);
            createAccInstance.transform.SetAsLastSibling();

        }
    }


    public void ShowPopupPanel(string title, string content)
    {
        GameObject popupPanelInstance = Instantiate(popupPanel, canvas.transform);
        popupPanelInstance.transform.SetAsLastSibling();
        popupPanelInstance.GetComponent<PopupPanelController>().SetContent(title, content);
    }
}