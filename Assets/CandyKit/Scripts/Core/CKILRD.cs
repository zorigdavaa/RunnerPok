using CandyKitSDK;
// using Firebase.Analytics;
using UnityEngine;

public class CKILRD
{
    private static bool _subscribed = false;
    public static void ListenForImpressionForFirebase()
    {
        // if (!_subscribed)
        // {
        //     MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += SendEventToFirebase;
        //     MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += SendEventToFirebase;
        //     MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += SendEventToFirebase;
        //     MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += SendEventToFirebase;
        //     // MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += SendEventToFirebase;
        //     _subscribed = true;
        // }
    }
    public static void ListenImpressionForTenjin()
    {
#if !UNITY_EDITOR
        if (CandyKit.m_Tenjin)
        {
            CandyKit.m_Tenjin.GetInstance().SubscribeAppLovinImpressions();
        }
#endif
    }
    // private static void SendEventToFirebase(string adUnitId, MaxSdkBase.AdInfo info)
    // {
    //     Parameter[] impressionParameters = new Parameter[]
    //     {
    //         new Parameter("ad_platform", "AppLovin"),
    //         new Parameter("ad_source", info.NetworkName),
    //         new Parameter("ad_unit_name", info.AdUnitIdentifier),
    //         new Parameter("ad_format", info.AdFormat),
    //         new Parameter("value", info.Revenue),
    //         new Parameter("currency", "USD"),
    //         new Parameter("country", MaxSdk.GetSdkConfiguration().CountryCode),
    //         // new Parameter("placement", info.Placement ?? "unknown"),  // Null check
    //         // new Parameter("creative_id", info.CreativeIdentifier ?? "unknown"),  // Null check
    //     };

    //     if (Firebase.FirebaseApp.DefaultInstance != null)
    //     {
    //         FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);  // Single semicolon
    //         Debug.Log("Firebase Impression");
    //     }
    // }
    // // Send parsed SKAdNetwork postback data to Google Analytics
    // public static void SendSKAdNetworkDataToGA(string skadData)
    // {
    //     // Log event in Firebase (Google Analytics)
    //     FirebaseAnalytics.LogEvent("skadnetwork_conversion",
    //         new Parameter("postback_data", skadData));

    //     Debug.Log("SKAdNetwork data sent to Google Analytics: " + skadData);
    // }
    // public static void LogPurchaseToFirebase(string productId, float revenue, string currency)
    // {
    //     try
    //     {
    //         FirebaseAnalytics.LogEvent(
    //             FirebaseAnalytics.EventPurchase,
    //             new Parameter(FirebaseAnalytics.ParameterItemName, productId),
    //             new Parameter(FirebaseAnalytics.ParameterValue, revenue),
    //             new Parameter(FirebaseAnalytics.ParameterCurrency, currency)
    //             );
    //         Debug.Log("Firebase Sent Purchase Event " + productId);
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogError("Failed to log purchase to Firebase: " + e.Message);
    //         throw;
    //     }
    // }
}
