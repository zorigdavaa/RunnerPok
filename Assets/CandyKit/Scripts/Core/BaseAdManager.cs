using System;
using CandyKitSDK;
using UnityEngine;
using UnityEngine.Events;

public class BaseAdManager : MonoBehaviour
{
    public virtual void HideBanner()
    {
        throw new NotImplementedException();
    }

    public virtual void ShowBanner()
    {
        throw new NotImplementedException();
    }

    public virtual void ShowInterstitial(string placement, UnityAction onSuccess)
    {
        throw new NotImplementedException();
    }

    public virtual void ShowRewardedVideo(string placement, CkRewardedAdCallback callback)
    {
        throw new NotImplementedException();
    }

    public virtual float GetBannerHeight()
    {
        throw new NotImplementedException();
    }

    public virtual void Initialize(CandyKitSettingsScriptableObject m_Settings)
    {
        throw new NotImplementedException();
    }
}
