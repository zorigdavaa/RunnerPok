using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoCanvas : MonoBehaviour
{
    public TextMeshProUGUI txtName;
    BaseItemUI itemUI;
    [SerializeField] Button btnWear;
    [SerializeField] Button btnUpgrade;
    [SerializeField] Image Icon;
    // Start is called before the first frame update
    void Start()
    {
        btnWear.onClick.AddListener(WearorRemove);
        btnUpgrade.onClick.AddListener(Upgrade);
    }

    private void Upgrade()
    {
        throw new NotImplementedException();
    }

    private void WearorRemove()
    {
        throw new NotImplementedException();
    }

    public void ShowInfoOf(BaseItemUI _ItemUI)
    {
        itemUI = _ItemUI;
        txtName.text = _ItemUI.data.name;
        Icon.sprite = _ItemUI.data.Icon;
    }
}
