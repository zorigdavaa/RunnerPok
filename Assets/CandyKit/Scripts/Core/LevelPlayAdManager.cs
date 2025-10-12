using System;
using CandyKitSDK;
using Unity.Services.LevelPlay;
using UnityEngine;
using UnityEngine.Events;

public class LevelPlayAdManager : BaseAdManager
{
    CandyKitSettingsScriptableObject m_Settings;
    LevelPlayRewardedAd RewardedAd;
    LevelPlayBannerAd bannerAd;
    private LevelPlayInterstitialAd interstitialAd;
    public override void Initialize(CandyKitSettingsScriptableObject m_Settings)
    {
        this.m_Settings = m_Settings;
        // LevelPlay.OnInitSuccess += SdkInitializationSuccessEvent;
        // LevelPlay.OnInitFailed += SdkInitializationFailedEvent;

        CreateRewardedAd();
        CreateBannerAd();
        CreateInterstitialAd();
        LoadRewardedAd();
        LoadInterstitialAd();
        Debug.LogError("LevelPlay Ad Manager Initialized");

    }
    public override void ShowBanner()
    {
        Debug.Log("CK--> Show Banner");
        bannerAd.ShowAd();
    }
    public override void HideBanner()
    {
        bannerAd.HideAd();
    }
    UnityAction interstitialCallback;
    public override void ShowInterstitial(string placement, UnityAction onSuccess)
    {
        interstitialCallback = onSuccess;
        //Show InterstitialAd, check if the ad is ready before showing
        if (interstitialAd.IsAdReady())
        {
            interstitialAd.ShowAd();
        }
        else
        {
            interstitialAd.LoadAd();
        }
    }
    CkRewardedAdCallback rewardedAdCallback;
    public override void ShowRewardedVideo(string placement, CkRewardedAdCallback callback)
    {
        rewardedAdCallback = callback;
        //Show RewardedAd, check if the ad is ready before showing
        if (RewardedAd.IsAdReady())
        {
            RewardedAd.ShowAd();
        }
        else
        {
            LoadRewardedAd();
        }
    }

    void CreateBannerAd()
    {
        LevelPlayBannerAd.Config config = new LevelPlayBannerAd.Config.Builder()
            .SetSize(LevelPlayAdSize.BANNER)
            .SetPosition(LevelPlayBannerPosition.BottomCenter)
            .SetPlacementName("placementName")
            .SetDisplayOnLoad(true)
            .SetRespectSafeArea(true)
            .Build();
        bannerAd = new LevelPlayBannerAd(m_Settings.Android.BannerAdUnitId, config);

        //Subscribe RewardedAd events
        bannerAd.OnAdLoaded += BannerOnAdLoadedEvent;
        bannerAd.OnAdLoadFailed += BannerOnAdLoadFailedEvent;
        bannerAd.OnAdDisplayed += BannerOnAdDisplayedEvent;
        bannerAd.OnAdDisplayFailed += BannerOnAdDisplayFailedEvent;
        bannerAd.OnAdClicked += BannerOnAdClickedEvent;
        bannerAd.OnAdCollapsed += BannerOnAdCollapsedEvent;
        bannerAd.OnAdLeftApplication += BannerOnAdLeftApplicationEvent;
        bannerAd.OnAdExpanded += BannerOnAdExpandedEvent;
    }
    public override float GetBannerHeight()
    {
        var size = bannerAd.GetAdSize();
        return size.Height;
    }


    void CreateRewardedAd()
    {
        //Create RewardedAd instance
        RewardedAd = new LevelPlayRewardedAd(m_Settings.Android.RewardedVideoAdUnitId);

        //Subscribe RewardedAd events
        RewardedAd.OnAdLoaded += RewardedOnAdLoadedEvent;
        RewardedAd.OnAdLoadFailed += RewardedOnAdLoadFailedEvent;
        RewardedAd.OnAdDisplayed += RewardedOnAdDisplayedEvent;
        RewardedAd.OnAdDisplayFailed += RewardedOnAdDisplayFailedEvent;
        RewardedAd.OnAdClicked += RewardedOnAdClickedEvent;
        RewardedAd.OnAdClosed += RewardedOnAdClosedEvent;
        RewardedAd.OnAdRewarded += RewardedOnAdRewarded;
        RewardedAd.OnAdInfoChanged += RewardedOnAdInfoChangedEvent;
    }



    void LoadRewardedAd()
    {
        //Load or reload RewardedAd     
        RewardedAd.LoadAd();
    }


    //Implement RewardedAd events
    void RewardedOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { rewardedAdCallback?.Invoke(false); LoadRewardedAd(); rewardedAdCallback = null; }
    void RewardedOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void RewardedOnAdClosedEvent(LevelPlayAdInfo adInfo) { rewardedAdCallback?.Invoke(false); LoadRewardedAd(); }
    void RewardedOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
    private void RewardedOnAdRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
    {
        rewardedAdCallback?.Invoke(true); LoadRewardedAd();
        rewardedAdCallback = null;
    }

    private void RewardedOnAdDisplayFailedEvent(LevelPlayAdInfo info, LevelPlayAdError error)
    {
        throw new NotImplementedException();
    }


    void BannerOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    void BannerOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdCollapsedEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdLeftApplicationEvent(LevelPlayAdInfo adInfo) { }
    void BannerOnAdExpandedEvent(LevelPlayAdInfo adInfo) { }
    private void BannerOnAdDisplayFailedEvent(LevelPlayAdInfo info, LevelPlayAdError error)
    {
        throw new NotImplementedException();
    }



    void CreateInterstitialAd()
    {
        //Create InterstitialAd instance
        interstitialAd = new LevelPlayInterstitialAd(m_Settings.Android.InterstitialAdUnitId);

        //Subscribe InterstitialAd events
        interstitialAd.OnAdLoaded += InterstitialOnAdLoadedEvent;
        interstitialAd.OnAdLoadFailed += InterstitialOnAdLoadFailedEvent;
        interstitialAd.OnAdDisplayed += InterstitialOnAdDisplayedEvent;
        interstitialAd.OnAdDisplayFailed += InterstitialOnAdDisplayFailedEvent;
        interstitialAd.OnAdClicked += InterstitialOnAdClickedEvent;
        interstitialAd.OnAdClosed += InterstitialOnAdClosedEvent;
        interstitialAd.OnAdInfoChanged += InterstitialOnAdInfoChangedEvent;
    }



    void LoadInterstitialAd()
    {
        //Load or reload InterstitialAd 	
        interstitialAd.LoadAd();
    }


    //Implement InterstitialAd events
    void InterstitialOnAdLoadedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdLoadFailedEvent(LevelPlayAdError ironSourceError) { }
    void InterstitialOnAdClickedEvent(LevelPlayAdInfo adInfo) { }
    void InterstitialOnAdDisplayedEvent(LevelPlayAdInfo adInfo) { interstitialCallback?.Invoke(); }
    private void InterstitialOnAdDisplayFailedEvent(LevelPlayAdInfo info, LevelPlayAdError error)
    {
        LoadInterstitialAd();
    }
    void InterstitialOnAdClosedEvent(LevelPlayAdInfo adInfo)
    {
        LoadInterstitialAd();
    }
    void InterstitialOnAdInfoChangedEvent(LevelPlayAdInfo adInfo) { }
}
