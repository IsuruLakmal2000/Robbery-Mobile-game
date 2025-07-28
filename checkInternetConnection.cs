using TMPro;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Purchasing;

public class CheckInternetConection : MonoBehaviour
{
    public GameObject noConnectionPanel;



    private Canvas canvas;

    public bool isCheckingConnectionComplete = false;
    public static CheckInternetConection instance;

    public bool connecionAvailble = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canvas = FindFirstObjectByType<Canvas>();

        // contactPanel = Resources.Load<GameObject>("Prefab/Panels/Contact Panel");
        // contactBtn = GameObject.FindGameObjectWithTag("ContactBtn").GetComponent<Button>();
    }

    private void Start()
    {

        // noConnectionPanel = Resources.Load<GameObject>("Prefab/Panels/No internet Connection Panel");

        StartCoroutine(CheckInternetConnection());

    }

    public IEnumerator CheckInternetConnection()
    {
        Debug.Log("Checking internet connection");
        isCheckingConnectionComplete = false;
        connecionAvailble = false;
        UnityWebRequest request = new UnityWebRequest("https://www.google.com");
        request.timeout = 10;
        yield return request.SendWebRequest();
        isCheckingConnectionComplete = true;
        Debug.Log("check status -------" + request.result);

        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            ShowNeedConectionPanel();
            Debug.Log("Internet connection is not available");
        }
        else if (request.result == UnityWebRequest.Result.Success)
        {
            connecionAvailble = true;
            Debug.Log("Internet connection is available");
        }
        else
        {
            ShowNeedConectionPanel();
        }


    }



    private void ShowNeedConectionPanel()
    {
        GameObject popupPanelLargeInstance = Instantiate(noConnectionPanel, canvas.transform);
        popupPanelLargeInstance.transform.SetAsLastSibling();
        GameObject nextBtnObj = popupPanelLargeInstance.transform.Find("button").gameObject;

        nextBtnObj.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
          {
              SceneManager.LoadScene(SceneManager.GetActiveScene().name);
              SoundManager.instance.PlayButtonClick();
              Destroy(popupPanelLargeInstance);
          });


    }


}