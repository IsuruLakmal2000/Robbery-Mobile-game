using System.Threading.Tasks;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class CreateAccountPopupPanel : MonoBehaviour
{

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] Button okBtn;
    [SerializeField] private TextMeshProUGUI warningTxt;


    void Start()
    {
        warningTxt.gameObject.SetActive(false);
        okBtn.onClick.AddListener(OnOkBtnPressedWrapper); // Use a wrapper method
    }

    private void OnOkBtnPressedWrapper()
    {
        // Call the async method and handle it without awaiting
        _ = OnOkBtnPressed();
    }

    public async Task OnOkBtnPressed()
    {
        Debug.Log("ok button pressed");
        string userInput = inputField.text;

        if (!string.IsNullOrEmpty(userInput))
        {
            warningTxt.gameObject.SetActive(true);
            warningTxt.text = "saving details.Please wait...";
            Debug.Log("nameee---------" + userInput);
            PlayerPrefs.SetString("UserName", userInput);
            OnLoginComplete();

        }
        else
        {
            warningTxt.gameObject.SetActive(true);
            warningTxt.text = "Please Enter valid name !";
            Debug.Log("Input is invalid!");
        }
    }
    private void OnLoginComplete()
    {
        Debug.Log("user data saved !!");
        warningTxt.text = "success !";

        PlayerPrefs.SetInt("IsFirstTime", 0);
        warningTxt.color = Color.green;
        Destroy(gameObject);
        // waitingTxt.gameObject.SetActive(false);
    }
}