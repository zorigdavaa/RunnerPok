using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CandyKitSDK
{
    [CreateAssetMenu(fileName = "Data", menuName = "CandyKit/CandyKitSettings", order = 1)]
    public class CandyKitSettingsScriptableObject : ScriptableObject
    {
        [Header("Meta")]
        public string FacebookAppID;
        public string FacebookAppClientToken;
        [Header("AppLovin")]
        public string MaxSDKKey;
        public bool SubmitFpsCritical = true;
        public bool SubmitFpsAverage = true;
        public float FpsCriticalThreshold = 20;
        [Space]
        public CkSettingsType Android;
        public CkSettingsType iOS;
    }

    [System.Serializable]
    public class CkSettingsType
    {
        [Header("Game Analytics")]
        public string GameAnalyticsGameKey;
        public string GameAnalyticsGameSecret;
        [Header("Tenjin")]
        public string TenjinSDKKey;
        [Header("Ad Unit IDs")]
        public string BannerAdUnitId;
        public string InterstitialAdUnitId;
        public string RewardedVideoAdUnitId;
        [Header("In-App Purchase")]
        public string NoAdsProductID;
        public List<CkProduct> ckProducts = new List<CkProduct>();
    }

    // [System.Serializable]
    // public class CkProduct
    // {
    //     public string ID;
    //     public ProductType ProductType;
    // }
}
