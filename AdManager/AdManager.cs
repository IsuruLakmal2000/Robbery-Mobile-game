using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string _rewardAdUnitId = "ca-app-pub-3940256099942544/5354046379";
    private InterstitialAd _interstitialAd;
    private RewardedInterstitialAd _rewardedInterstitialAd;

    public static AdManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void Start()
    {

        MobileAds.Initialize((InitializationStatus initStatus) =>
        {

        });
        LoadInterstitialAd();
        LoadRewardedInterstitialAd();
    }

    public void LoadInterstitialAd()
    {

        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");


        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {

                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
                RegisterEventHandlers(_interstitialAd);
            });
    }

    public void LoadRewardedInterstitialAd()
    {

        if (_rewardedInterstitialAd != null)
        {
            _rewardedInterstitialAd.Destroy();
            _rewardedInterstitialAd = null;
        }

        Debug.Log("Loading the rewarded interstitial ad.");
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");


        RewardedInterstitialAd.Load(_rewardAdUnitId, adRequest,
            (RewardedInterstitialAd ad, LoadAdError error) =>
            {

                if (error != null || ad == null)
                {
                    Debug.LogError("rewarded interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedInterstitialAd = ad;
                RegisterEventHandlersRewarded(_rewardedInterstitialAd);
            });
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
        }
    }

    public void ShowRewardedInterstitialAd(string rewardType, int rewardCount)
    {
        Debug.Log("isnide rewarded");
        const string rewardMsg =
           "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            Debug.Log(" showing");
            _rewardedInterstitialAd.Show((Reward reward) =>
            {
                Rewarded(rewardType, rewardCount);
                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
        else
        {
            //Time.timeScale = 1f;
        }
    }

    private void Rewarded(string type, int amount)
    {
        switch (type)
        {
            case "watch_ad_money_gameplay":
                GameRobbedMoney.instance.IncreaseMoneyWhenCollect(amount);
                break;
            case "watch_ad_gem_gameplay":
                GameRobbedMoney.instance.IncreaseGemWhenCollect(amount);
                break;


        }
    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () =>
        {

        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("full screen content closed");
            _interstitialAd.Destroy();
            LoadInterstitialAd();
            Time.timeScale = 1f;
        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            _interstitialAd.Destroy();
            LoadInterstitialAd();
            Time.timeScale = 1f;
        };
    }

    private void RegisterEventHandlersRewarded(RewardedInterstitialAd ad)
    {

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            _rewardedInterstitialAd.Destroy();
            LoadRewardedInterstitialAd();
            Time.timeScale = 1f;
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            _rewardedInterstitialAd.Destroy();
            LoadRewardedInterstitialAd();
            Time.timeScale = 1f;
        };

    }
}