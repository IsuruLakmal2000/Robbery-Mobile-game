using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    // Ad unit IDs from AdMob
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5354046379";
    private string interstitialAdUnitId = "ca-app-pub-9764584713102923/9314619367";

    //  private RewardedAd rewardedAd;
    private InterstitialAd _interstitialAd;

    private RewardedInterstitialAd _rewardedInterstitialAd;

    public static AdManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }




    private void Start()
    {
        // Initialize AdMob
        MobileAds.Initialize(initStatus => { });
        LoadInterstitialAd();
        LoadRewardedInterstitialAd();
    }

    private void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }
        Debug.Log("Loading the interstitial ad.");
        var adRequest = new AdRequest();

        InterstitialAd.Load(interstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
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
        RewardedInterstitialAd.Load(rewardedAdUnitId, adRequest,
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
                RegisterEventHandlersReward(_rewardedInterstitialAd);
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
        }
    }

    public void ShowRewardedInterstitialAd()
    {
        const string rewardMsg =
            "Rewarded interstitial ad rewarded the user. Type: {0}, amount: {1}.";

        if (_rewardedInterstitialAd != null && _rewardedInterstitialAd.CanShowAd())
        {
            _rewardedInterstitialAd.Show((Reward reward) =>
            {

                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }
    private void RegisterEventHandlers(InterstitialAd interstitialAd)
    {


        interstitialAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () =>
        {
            _interstitialAd.Destroy();
            LoadInterstitialAd();
        };

        interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            _interstitialAd.Destroy();
            LoadInterstitialAd();
        };
    }
    private void RegisterEventHandlersReward(RewardedInterstitialAd rewardedAd)
    {
        rewardedAd.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        // Raised when the ad closed full screen content.
        rewardedAd.OnAdFullScreenContentClosed += () =>
        {
            _interstitialAd.Destroy();
            LoadRewardedInterstitialAd();
        };

        rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
        {
            _rewardedInterstitialAd.Destroy();
            LoadRewardedInterstitialAd();
        };
    }



}
