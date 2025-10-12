using System.Collections;
using System.Collections.Generic;
using CandyKitSDK;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Initializer : MonoBehaviour
{

    void Start()
    {
        CandyKit.Initialize(OnReady);
    }


    void OnReady()
    {
        CandyKit.ShowBanner();
        SceneManager.LoadScene("Main");
        Debug.Log("On ready");
    }

}