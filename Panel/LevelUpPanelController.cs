using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpPanelController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private Button closeBtn;

    void Start()
    {
        closeBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            Destroy(gameObject);
        });
    }
    public void SetLevel(int level)
    {
        levelTxt.text = level.ToString();
    }

}