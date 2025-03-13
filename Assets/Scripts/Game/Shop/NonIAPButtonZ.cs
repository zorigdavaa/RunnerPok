using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public class NonIAPButtonZ : MonoBehaviour
{
    // public CkProduct product;
    public Button Button;
    public InGameProduct Product;
    public CurrencyType currencyType;
    public TextMeshProUGUI priceText, NameText;
    // Start is called before the first frame update
    void Start()
    {
        if (Button == null)
        {
            Button = GetComponent<Button>();
        }
        Button.onClick.AddListener(BuyProduct);
        priceText.text = Product.Price.ToString();
        NameText.text = Product.Amount.ToString();
    }

    // Update is called once per frame
    void BuyProduct()
    {
        if (currencyType == CurrencyType.Coin)
        {
            if (GameManager.Instance.Coin > Product.Price)
            {
                Product.Logic?.Invoke();
                GameManager.Instance.Coin -= Product.Price;
            }
        }
        else if (currencyType == CurrencyType.Gem)
        {

        }

    }

    // private void OnComplete(bool success)
    // {
    //     product.Logic?.Invoke();
    // }
}
