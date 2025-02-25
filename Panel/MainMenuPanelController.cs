using UnityEngine;

public class MainMenuPanelController : MonoBehaviour
{

    [SerializeField] private GameObject popupPanel;
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
        canvas = FindObjectOfType<Canvas>();

    }


    public void ShowPopupPanel(string title, string content)
    {
        GameObject popupPanelInstance = Instantiate(popupPanel, canvas.transform);
        popupPanelInstance.transform.SetAsLastSibling();
        popupPanelInstance.GetComponent<PopupPanelController>().SetContent(title, content);
    }
}