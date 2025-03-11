using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public class NonIAPButtonZ : MonoBehaviour
{
    public CkProduct product;
    public Button Button;
    public int Price;
    // Start is called before the first frame update
    void Start()
    {
        if (Button == null)
        {
            Button = GetComponent<Button>();
        }
        Button.onClick.AddListener(BuyProduct);
    }

    // Update is called once per frame
    void BuyProduct()
    {
        if (GameManager.Instance.Coin > Price)
        {
            product.Logic?.Invoke();
            GameManager.Instance.Coin -= Price;
        }
    }

    // private void OnComplete(bool success)
    // {
    //     product.Logic?.Invoke();
    // }
}
