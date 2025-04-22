using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelController : MonoBehaviour
{

    [SerializeField] private Image rewardImg;
    [SerializeField] private TextMeshProUGUI rewardCoundText;
    [SerializeField] private Button collectBtn;



    void Start()
    {
        collectBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlayButtonClick();
            Destroy(gameObject);
        });

    }

    public void SetRewardDetails(string rewardImgName, int rewardCount)
    {
        rewardImg.sprite = Resources.Load<Sprite>("Sprites/Task/icons/" + rewardImgName);
        rewardCoundText.text = rewardCount.ToString();
        StartCoroutine(AnimateRewardCount(rewardCount));
    }
    public void SetAvatarFrameUnlockingDetails(string imgName)
    {
        if (imgName.StartsWith("a"))
        {
            rewardImg.sprite = Resources.Load<Sprite>("Sprites/avatars/" + imgName);
            rewardCoundText.text = "You unlocked a new avatar!";
        }
        else if (imgName.StartsWith("f"))
        {
            rewardImg.sprite = Resources.Load<Sprite>("Sprites/frames/" + imgName);
            rewardCoundText.text = "You unlocked a new frame!";
        }

        // rewardCoundText.text = rewardCount.ToString();
        //  StartCoroutine(AnimateRewardCount(rewardCount));
    }

    private IEnumerator AnimateRewardCount(int targetValue)
    {
        int currentValue = int.Parse(rewardCoundText.text); // Get the current value
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / 1f);
            int newValue = Mathf.RoundToInt(Mathf.Lerp(currentValue, targetValue, t));
            rewardCoundText.text = newValue.ToString();
            yield return null;
        }

        // Ensure the final value is set
        rewardCoundText.text = targetValue.ToString();
    }


}