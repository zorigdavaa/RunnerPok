using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public class NonIAPButtonZ : MonoBehaviour
{
    public CkProduct product;
    public Button Button;
    public int Price;
    public string Name;
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
        priceText.text = Price.ToString();
        NameText.text = Name.ToString();
    }

    // Update is called once per frame
    void BuyProduct()
    {
        if (currencyType == CurrencyType.Coin)
        {
            if (GameManager.Instance.Coin > Price)
            {
                product.Logic?.Invoke();
                GameManager.Instance.Coin -= Price;
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
