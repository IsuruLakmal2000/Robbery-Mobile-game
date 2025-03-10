using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class PlayGamesManager : MonoBehaviour
{

    public TextMeshProUGUI name;
    public static PlayGamesManager instance;
    public string userName = "";
    public string userId = "";
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }
    void Start()
    {
        SignIn();
    }


    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            userName = PlayGamesPlatform.Instance.GetUserDisplayName();
            Debug.Log("Sign in success " + name);
            userId = PlayGamesPlatform.Instance.GetUserId();
            Debug.Log("User ID " + userId);
            name.text = userName.ToString();

        }
        else
        {
            Debug.Log("Sign in failed" + status);
        }
    }
}