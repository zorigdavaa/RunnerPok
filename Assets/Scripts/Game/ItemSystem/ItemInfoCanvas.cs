using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public class ItemInfoCanvas : MonoBehaviour
{
    private static ItemInfoCanvas _instance;
    public static ItemInfoCanvas Instance
    {
        get
        {
            if (_instance == null)
            {
                return FindObjectOfType<ItemInfoCanvas>(true);
            }
            return _instance;
        }
        set { _instance = value; }
    }

    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtDescription;
    public TextMeshProUGUI Info;
    BaseItemUI itemUI;
    [SerializeField] Button btnWear;
    [SerializeField] Button btnUpgrade;
    [SerializeField] Image Icon;
    public void Awake()
    {
        Instance = this;
        // gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        btnWear.onClick.AddListener(WearorRemove);
        btnUpgrade.onClick.AddListener(Upgrade);
    }

    private void Upgrade()
    {
        itemUI.Upgrade();
    }

    private void WearorRemove()
    {
        if (!itemUI.currentSlot.WearSlot)
        {
            var freeSlot = PlayerBuffItems.Instance.GetByTypeFromEquipedFree(itemUI.data.Where);
            if (freeSlot)
            {
                freeSlot.AddItem(itemUI);
            }
            else
            {
                Debug.Log("Already Worm This " + itemUI.data.Where);
                var UsedSlot = PlayerBuffItems.Instance.GetByTypeFromEquiped(itemUI.data.Where);
                Z.CanM.ShowPlusOne(UsedSlot.transform.position, "Already Worn", Color.red);
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
        txtDescription.text = itemUI.data.GetDescription();
        Info.text = itemUI.data.GetInfo();
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
