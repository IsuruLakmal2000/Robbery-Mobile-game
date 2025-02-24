using UnityEngine;
using TMPro;

public class XPDisplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    private void Start()
    {
        XPSystem.Instance.OnLevelUp += UpdateUI;
        UpdateUI(XPSystem.Instance.currentLevel);
    }

    private void UpdateUI(int newLevel)
    {
        levelText.text = XPSystem.Instance.currentLevel.ToString();
        xpText.text = $" {XPSystem.Instance.currentXP}/{XPSystem.Instance.xpToNextLevel}";
    }

    private void Update()
    {
        xpText.text = $"{XPSystem.Instance.currentXP}/{XPSystem.Instance.xpToNextLevel}";
    }
}
