using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyKitSDK;
using ZPackage;
using System;

public class CKProgressionEvent : MonoBehaviour
{
    bool isInited = false;
    public void Init()
    {
        Debug.LogError("Delete this and uncomment code");
        if (!isInited)
        {
            GameManager.Instance.StateChanged += OnGameStateChange;
            isInited = true;
        }
    }

    private void OnGameStateChange(object sender, GameState state)
    {
        if (!CandyKit.IsInitialized())
        {
            Debug.Log("Candy not ready");
            return;
        }

        if (state == GameState.Starting)
        {
            CandyKit.NotifyLevelZero();
        }
        if (state == GameState.Playing)
        {
            CandyKit.NotifyLevelStarted(GameManager.Instance.Level);
        }
        else if (state == GameState.LevelCompleted)
        {
            CandyKit.NotifyLevelCompleted(GameManager.Instance.Level);
        }
        else if (state == GameState.GameOver)
        {
            CandyKit.NotifyLevelFailed(GameManager.Instance.Level);

        }
        // else if (state == GameState.Revive)
        // {
        //     CandyKit.NotifyLevelFailed(GameManager.Instance.Level);
        //     Debug.Log("Progression Level Revive " + GameController.Level);
        // }
    }


    // void OnGameStateChange(GameState state)
    // {
    //     if (!CandyKit.IsInitialized())
    //     {
    //         Debug.Log("Candy not ready");
    //         return;
    //     }

    //     if (state == GameState.Starting)
    //     {
    //         CandyKit.NotifyLevelZero();
    //         Debug.Log("Progression Level Starting " + GameController.Level);
    //     }
    //     if (state == GameState.Playing)
    //     {
    //         CandyKit.NotifyLevelStarted(GameController.Level);
    //         Debug.Log("Progression Level Start " + GameController.Level);
    //     }
    //     else if (state == GameState.LevelCompleted)
    //     {
    //         CandyKit.NotifyLevelCompleted(GameController.Level);
    //         Debug.Log("Progression Level Complete " + GameController.Level);
    //     }
    //     else if (state == GameState.GameOver)
    //     {
    //         CandyKit.NotifyLevelFailed(GameController.Level);
    //         Debug.Log("Progression Game Over " + GameController.Level);
    //     }
    //     else if (state == GameState.Revive)
    //     {
    //         CandyKit.NotifyLevelFailed(GameController.Level);
    //         Debug.Log("Progression Level Revive " + GameController.Level);
    //     }
    // }


    // void OnDestroy()
    // {
    //     if (isInited)
    //     {
    //         GameController.OnGameStateChange -= OnGameStateChange;
    //         isInited = false;
    //     }
    // }
}
