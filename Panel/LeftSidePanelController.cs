using UnityEngine;
using UnityEngine.UI;

public class LeftSidePanelController : MonoBehaviour
{
    [SerializeField] private Button achievemntsBtn;
    [SerializeField] private GameObject achievemntsPanelPrefab;
    [SerializeField] private Canvas canvas;


    void Start()
    {
        achievemntsBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            GameObject achievemntsPanel = Instantiate(achievemntsPanelPrefab, canvas.transform);
            achievemntsPanel.transform.SetAsLastSibling();
            achievemntsPanel.transform.Find("background/close btn").GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.instance.PlayButtonClick();
                Destroy(achievemntsPanel);
            });
        });
    }

}