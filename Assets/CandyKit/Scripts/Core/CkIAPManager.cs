using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using CandyKitSDK;
using UnityEngine.Purchasing.Extension;

public class CkIAPManager : MonoBehaviour, IDetailedStoreListener
{
    private IStoreController controller;
    private IExtensionProvider extensions;
    private IAppleExtensions m_AppleExtensions;
    private CandyKitSettingsScriptableObject m_Settings;
    public delegate void OnPurchaseCompleted(bool success);
    private event OnPurchaseCompleted m_OnPurchaseCompleted;
    public bool isIAPInitialized = false;
    public string ErrorMessage = "";

    public void Initialize()
    {
        DontDestroyOnLoad(gameObject);

        m_Settings = CandyKit.Settings;
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

#if UNITY_IOS
        builder.AddProduct(m_Settings.iOS.NoAdsProductID, ProductType.NonConsumable);
        foreach (var item in m_Settings.iOS.ckProducts)
        {
            builder.AddProduct(item.ID, item.ProductType);
        }

#else
        builder.AddProduct(m_Settings.Android.NoAdsProductID, ProductType.NonConsumable);
        foreach (var item in m_Settings.Android.ckProducts)
        {
            builder.AddProduct(item.ID, item.ProductType);
        }
#endif

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("CK--> IAPMANGER initialized");
        this.controller = controller;
        this.extensions = extensions;
        isIAPInitialized = true;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // OnInitializeFailed(error, null);
        ErrorMessage = "CK--> IAP initialization failed: " + ", " + error;
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError("CK--> IAP initialization failed: " + message + ", " + error);
        ErrorMessage = "CK--> IAP initialization failed: " + message + ", " + error;
    }

    public void RestorePurchase(OnPurchaseCompleted onPurchaseCompleted)
    {
        m_OnPurchaseCompleted = onPurchaseCompleted;
#if UNITY_IOS
        m_AppleExtensions.RestoreTransactions(OnRestore);
#else
        m_OnPurchaseCompleted(false);
#endif
    }

    private void OnRestore(bool success, string error)
    {
        string message = "";
        if (success)
        {
            PlayerPrefs.SetInt(CkConstants.PremiumUserPref, 1);
            m_OnPurchaseCompleted(true);
            message = "CK-> Restored purchase successfully";
        }
        else
        {
            message = "CK-> Restore failed with error: " + error;
        }

        Debug.Log(message);
    }

    public void OnPurchaseClicked(string productId, OnPurchaseCompleted onPurchaseCompleted)
    {
        if (productId == null)
        {
            Debug.LogError("CK--> productId is null");
            return;
        }

        if (onPurchaseCompleted == null)
        {
            Debug.LogError("CK--> onPurchaseCompleted is null");
            return;
        }
        if (controller == null)
        {
            Debug.LogError("CK--> controller is null");
            return;
        }
        m_OnPurchaseCompleted = onPurchaseCompleted;
        controller.InitiatePurchase(productId);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (e.purchasedProduct.definition.id == m_Settings.iOS.NoAdsProductID)
            {
                PlayerPrefs.SetInt(CkConstants.PremiumUserPref, 1);
            }
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (e.purchasedProduct.definition.id == m_Settings.Android.NoAdsProductID)
            {
                PlayerPrefs.SetInt(CkConstants.PremiumUserPref, 1);
            }
        }
        m_OnPurchaseCompleted(true);

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        m_OnPurchaseCompleted(false);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        m_OnPurchaseCompleted(false);
    }
    public void IncreaseRevenueCount(float amount)
    {
        float Count = PlayerPrefs.GetFloat(CkConstants.IAPRevenuePref, 0);
        Count += amount;
        PlayerPrefs.SetFloat(CkConstants.IAPRevenuePref, Count);
    }
    public int GetIAPConversionValue()
    {
        float Count = PlayerPrefs.GetFloat(CkConstants.IAPRevenuePref, 0);
        switch (Count)
        {
            case < 0.5f: return 0;
            case < 1: return 8;
            case < 3: return 16;
            case < 7: return 16;
            case < 12: return 24;
            default: return 36;
        }
    }
}
