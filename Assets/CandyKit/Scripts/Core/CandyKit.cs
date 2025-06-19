using UnityEngine;
using UnityEngine.Events;


// #if CANDYKIT || UNITY_ANDROID || UNITY_IOS
// using GameAnalyticsSDK;
// #endif

namespace CandyKitSDK
{
    public class CandyKit
    {
        private const string version = "1.1.6";
        private const float m_ReadinessWaitDuration = 2f;
        private static bool m_IsDebug = false;
        private static bool m_IsInitialized = false;
        private static CandyKitObject m_Instance;
        private static CkIAPManager m_IAPManager;
        private static CandyKitSettingsScriptableObject m_Settings;
        public static CandyKitSettingsScriptableObject Settings
        {
            get
            {
                if (m_Settings == null)
                {
                    m_Settings = Resources.Load<CandyKitSettingsScriptableObject>("CandyKit/CandyKitSettings");
                }

                return m_Settings;
            }
        }

        public delegate void OnCandyKitReady();
        private static event OnCandyKitReady OnReady;

        #region -- API --

        public static void Initialize(OnCandyKitReady onReady, bool isDebug = false)
        {
            m_IsDebug = isDebug;

            if (!m_IsInitialized)
            {
                if (m_IsDebug)
                {
                    Debug.Log("CK--> Initialize CandyKit");
                }

                m_IsInitialized = true;
                OnReady = onReady;

                m_Settings = Resources.Load<CandyKitSettingsScriptableObject>("CandyKit/CandyKitSettings");

                GameObject instanceObject = new("CandyKitObject");
                m_Instance = instanceObject.AddComponent<CandyKitObject>();
                m_Instance.Initialize(m_ReadinessWaitDuration, m_Settings, OnReady);

                InitializeUnityGamingService();
            }
            else
            {
                if (m_IsDebug)
                {
                    Debug.Log("CK--> already initialized!");
                }
            }
        }

        public static void NotifyLevelZero()
        {
            if (m_IsDebug)
            {
                Debug.Log("CK--> NotifyLevelZero");
            }
            if (!m_IsInitialized)
            {
                return;
            }


            // #if CANDYKIT || UNITY_IOS || UNITY_ANDROID
            //             if (PlayerPrefs.GetInt("LevelZero", 0) == 0)
            //             {
            //                 PlayerPrefs.SetInt("LevelZero", 1);
            //                 GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_000");
            //                 GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_000");
            //             }
            // #endif
        }

        public static void NotifyLevelStarted(int level)
        {
            if (m_IsDebug)
            {
                Debug.Log("CandyKit-> NotifyLevelStarted: " + level);
            }

            if (!m_IsInitialized)
            {
                return;
            }

            NotifyLevelZero();

            // #if CANDYKIT || UNITY_IOS || UNITY_ANDROID
            //             GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_" + level.ToString("000"));
            // #endif
        }

        public static void NotifyLevelCompleted(int level)
        {
            if (m_IsDebug)
            {
                Debug.Log("CK--> NotifyLevelCompleted: " + level);
            }

            if (!m_IsInitialized)
            {
                return;
            }

            // #if CANDYKIT || UNITY_IOS || UNITY_ANDROID
            //             GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_" + level.ToString("000"));
            // #endif
        }

        public static void NotifyLevelFailed(int level)
        {
            if (m_IsDebug)
            {
                Debug.Log("CK--> NotifyLevelFailed: " + level);
            }

            if (!m_IsInitialized)
            {
                return;
            }

            // #if CANDYKIT || UNITY_IOS || UNITY_ANDROID
            //             GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level_" + level.ToString("000"));
            // #endif
        }

        public static void BuyNoAds(CkIAPManager.OnPurchaseCompleted onPurchaseCompleted)
        {
            if (m_IsDebug)
            {
                Debug.Log("CK--> Buy No Ads");
            }

            if (!m_IsInitialized)
            {
                onPurchaseCompleted(true);
                return;
            }

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                BuyProduct(m_Settings.iOS.NoAdsProductID, onPurchaseCompleted);
            }
            else
            {
                BuyProduct(m_Settings.Android.NoAdsProductID, onPurchaseCompleted);
            }
        }

        public static void BuyProduct(string productId, CkIAPManager.OnPurchaseCompleted onPurchaseCompleted)
        {
            if (m_IsDebug)
            {
                Debug.Log("CK--> Buy Product: " + productId);
            }
            if (!m_IsInitialized)
            {
                Debug.Log("CK--> CandyKit not initialized, cannot buy product: " + productId);
                return;
            }

            if (!m_IAPManager.isIAPInitialized)
            {
                onPurchaseCompleted(false);
                return;
            }
            m_IAPManager.OnPurchaseClicked(productId, onPurchaseCompleted);
        }

        public static void RestorePurchase(CkIAPManager.OnPurchaseCompleted onPurchaseCompleted)
        {
            if (m_IsDebug)
            {
                Debug.Log("CK--> Restore Purchase");
            }

            if (!m_IsInitialized)
            {
                onPurchaseCompleted(true);
                return;
            }

            // m_Instance.ckIAPManager.RestorePurchase(onPurchaseCompleted);
            m_IAPManager.RestorePurchase(onPurchaseCompleted);
        }

        #endregion

        // public static void RequestReview()
        // {
        //     if (!m_IsInitialized)
        //     {
        //         return;
        //     }

        //     m_Instance.ckInAppReviewObject.LaunchReview();
        // }

        #region -- PRIVATE --


        private static void InitializeUnityGamingService()
        {
            GameObject gamingServiceGo = new("CkUnityGamingServiceInitializer");
            CkUnityGamingServiceObject gamingServiceObject = gamingServiceGo.AddComponent<CkUnityGamingServiceObject>();
            gamingServiceObject.Initialize(OnGamingServiceInitializationSuccess, OnGamingServiceInitializationFailed);
        }


        private static void OnGamingServiceInitializationSuccess()
        {
            Debug.Log("CK--> UGS init Success");
            GameObject obj = new("CkIAPManager");
            m_IAPManager = obj.AddComponent<CkIAPManager>();
            m_IAPManager.Initialize();
        }

        private static void OnGamingServiceInitializationFailed(string error)
        {
            // Debug.Log("CK-> Gaming Service Initialization failed with error: " + error);
            Debug.Log("CK--> UGS init Failed " + error);
        }

        #endregion
    }
}
