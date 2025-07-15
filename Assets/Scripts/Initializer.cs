using System.Collections;
using System.Collections.Generic;
using CandyKitSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

#if SW_STAGE_STAGE10_OR_ABOVE
using SupersonicWisdomSDK;
#endif

public class Initializer : MonoBehaviour
{

#if SW_STAGE_STAGE10_OR_ABOVE
    const float SecondsToWait = 3f;
#else
    const float SecondsToWait = 1f;
#endif


    float timer = 0f;
    bool mainSceneLoaded = false;

#if SW_STAGE_STAGE10_OR_ABOVE
    void Awake()
    {
        // Subscribe
        SupersonicWisdom.Api.AddOnReadyListener(OnSupersonicWisdomReady);
        // Then initialize
        SupersonicWisdom.Api.Initialize();

    }
#endif
    private void Start()
    {
        CandyKit.Initialize(LoadMainScene);
    }

    void OnSupersonicWisdomReady()
    {

        print("-------------- Wisdom ready. --------------");
        LoadMainScene();

    }

    void LoadMainScene()
    {
        if (mainSceneLoaded) return;

        // ABTestManager.Instance.Init();
        SceneManager.LoadScene("Main");
        mainSceneLoaded = true;
        enabled = false;
    }

#if SW_STAGE_STAGE10_OR_ABOVE
    void OnDestroy()
    {
        SupersonicWisdom.Api.RemoveOnReadyListener(OnSupersonicWisdomReady);
    }
#endif

}
//Section one
//choose Obs Collect Fight choose obs fight 

//Section two
//choose collect fight choose fight
//3 Level oroh
//Item speed iig zowhon moveSpeed bolgoh
// Attack Speediig Item Speedeer solih