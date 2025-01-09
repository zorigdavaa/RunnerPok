using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;

// #if !UNITY_EDITOR
// using GameAnalyticsSDK;
// #endif

namespace CandyKitSDK
{
    public class CandyKitObject : MonoBehaviour
    {
        private CandyKitSettingsScriptableObject m_Settings;
        public CkInAppReviewObject ckInAppReviewObject;
        public CkIAPManager ckIAPManager;
        // CKSpecialEvents cKSpecialEvents;
        private bool IsLocked = true;

        public void Initialize(float waitDuration, CandyKitSettingsScriptableObject settings, CandyKit.OnCandyKitReady onReady)
        {
            DontDestroyOnLoad(gameObject);

            m_Settings = settings;

            StartCoroutine(WaitForReadiness(waitDuration, onReady));

        }

        private void RestartSdkObjects()
        {
            Debug.Log("CK-> Restarting SDK Objects");
            CreateFacebookObject();
            CreateInAppReviewObject();
        }

        private void CreateFacebookObject()
        {
            GameObject fbObjectPrefab = Resources.Load<GameObject>("CkFacebookObject");
            Instantiate(fbObjectPrefab);
        }

        private void CreateInAppReviewObject()
        {
            // ckInAppReviewObject = Instantiate(Resources.Load<CkInAppReviewObject>("CkInAppReviewObject"));
            // ckInAppReviewObject.Initialize();
        }
        // private void CreateCkSpecialEvent()
        // {
        //     if (m_Settings.SubmitFpsAverage || m_Settings.SubmitFpsCritical)
        //     {
        //         GameObject obj = new GameObject("CKSpecialEvent");
        //         cKSpecialEvents = obj.AddComponent<CKSpecialEvents>();
        //         DontDestroyOnLoad(obj);
        //     }
        // }


        private IEnumerator WaitForReadiness(float duration, CandyKit.OnCandyKitReady onReady)
        {

            while (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                yield return null;
            }

            print("UGS Initialized");

// #if !UNITY_EDITOR
//             while (!GameAnalytics.IsRemoteConfigsReady())
//             {
//                 yield return null;
//             }
// #endif

            ABTestManager.Instance.Init();

            RestartSdkObjects();
            // CreateCkSpecialEvent();
            IsLocked = false;

            onReady();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (!IsLocked)
            {
                Debug.Log("CK-> On Scene Loaded");
                RestartSdkObjects();
            }
        }
    }
}
