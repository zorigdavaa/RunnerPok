using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoCanvas : MonoBehaviour
{
    public static ItemInfoCanvas Instance;
    public TextMeshProUGUI txtName;
    BaseItemUI itemUI;
    [SerializeField] Button btnWear;
    [SerializeField] Button btnUpgrade;
    [SerializeField] Image Icon;
    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
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
        if (!itemUI.currentSlot.WearSlot)
        {
            var freeSlot = PlayerBuffItems.Instance.GetByTypeFromEquiped(itemUI.data.Where);
            if (freeSlot)
            {
                freeSlot.AddItem(itemUI);
            }
            else
            {
                Debug.Log("Already Worm This " + itemUI.data.Where);
            }
        }
        else
        {
            var freeSlot = PlayerBuffItems.Instance.GetFreefromUnEquip();
            if (freeSlot)
            {
                freeSlot.AddItem(itemUI);
            }
        }
        gameObject.SetActive(false);
    }

    public void ShowInfoOf(BaseItemUI _ItemUI)
    {
        gameObject.SetActive(true);
        itemUI = _ItemUI;
        txtName.text = _ItemUI.data.name;
        Icon.sprite = _ItemUI.data.Icon;
        if (_ItemUI.currentSlot.WearSlot)
        {
            btnWear.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Remove";
        }
        else
        {
            btnWear.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Wear";
        }
    }
}