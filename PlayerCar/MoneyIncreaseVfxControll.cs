using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // if you're using TextMeshPro

public class MoneyPopup : MonoBehaviour
{
    public TextMeshProUGUI moneyText;


    public void Setup(int amount)
    {
        moneyText.text = $"+{amount}";
        // You can set icon here if dynamic
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        // Vector3 moveTarget = transform.position + new Vector3(0, 50f, 0);
        float duration = 0.5f;
        float elapsed = 0f;
        //  CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        while (elapsed < duration)
        {
            // transform.position = Vector3.Lerp(transform.position, moveTarget, Time.deltaTime * 2f);
            //  canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
