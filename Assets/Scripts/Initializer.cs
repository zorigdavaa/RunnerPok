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
    //Todo 
    // Coin Manager hiih Coinii toog level file deer ogood tootoi ijil coin spawn hiih
    // Deeguur ywdag haalttai zam hiih 
    //Power Up system hiih Jump, Fly, Magnet, Shield, Double Coins, Speed Boost, Extra Life, Slow Motion, Score Multiplier, Health Regen, Invincibility, Time Freeze, Auto-Collect, Super Jump, Dash, Coin Magnet, Health Boost, Damage Boost, Experience Boost, Loot Box, Extra Score, Combo Multiplier, Shield Recharge, Speed Burst, Double Jump

}