using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupPanelController : MonoBehaviour
{


    [SerializeField] private Button closeBtn;
    [SerializeField] private TextMeshProUGUI contentTxt;
    [SerializeField] private TextMeshProUGUI titleTxt;



    void Start()
    {
        closeBtn.onClick.AddListener(ClosePanel);
    }

    public void SetContent(string title, string content)
    {
        titleTxt.text = title;
        contentTxt.text = content;
    }




    private void ClosePanel()
    {
        Destroy(gameObject);
    }
}