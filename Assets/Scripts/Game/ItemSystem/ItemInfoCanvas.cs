using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                return FindFirstObjectByType<ItemInfoCanvas>(FindObjectsInactive.Include);
            }
            return _instance;
        }
        set { _instance = value; }
    }
    public List<GameObject> Infos;
    public TextMeshProUGUI txtName;
    public TextMeshProUGUI txtLVL;
    public TextMeshProUGUI txtDescription;
    public TextMeshProUGUI Info;
    ItemInstanceUI itemUI;
    [SerializeField] Button btnWear;
    [SerializeField] Button btnUpgrade;
    [SerializeField] Image Icon;
    public List<StringSpritePair> IconPair;
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
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        foreach (var item in Infos)
        {
            item.gameObject.SetActive(false);
        }
    }

    private void Upgrade()
    {
        Debug.Log("upgrade");
        itemUI.Upgrade();
        RefreshData(itemUI);
    }

    private void WearorRemove()
    {
        if (itemUI.currentSlot.isUnequipSlot) // Wearing
        {
            var freeSlot = PlayerBuffItems.Instance.GetByTypeFromEquipedFree(itemUI.data.Where);
            if (freeSlot)
            {
                if (Z.Player.HasMoney(itemUI.Price))
                {
                    Z.Player.UseMoney(itemUI.Price);
                    freeSlot.AddItem(itemUI);
                }
                else
                {
                    Z.CanM.ShowPlusOne(itemUI.currentSlot.transform.position, "Need More Coin", Color.red);
                }
            }
            else
            {
                Debug.Log("Already Worm This " + itemUI.data.Where);
                var UsedSlot = PlayerBuffItems.Instance.GetByTypeFromEquiped(itemUI.data.Where);
                Z.CanM.ShowPlusOne(UsedSlot.transform.position, "Already Worn", Color.red);
            }
        }
        else // UnWearing
        {
            var freeSlot = PlayerBuffItems.Instance.GetFreefromUnEquip();
            if (freeSlot)
            {
                Z.Player.GiveMoney(itemUI.Price);
                freeSlot.AddItem(itemUI);
            }
        }
        gameObject.SetActive(false);
    }

    public void ShowInfoOf(ItemInstanceUI _ItemUI)
    {
        gameObject.SetActive(true);
        itemUI = _ItemUI;
        RefreshData(_ItemUI);
        if (_ItemUI.currentSlot.isUnequipSlot)
        {
            btnWear.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Wear";
        }
        else
        {
            btnWear.transform.GetComponentInChildren<TextMeshProUGUI>().text = "Remove";
        }
    }

    private void RefreshData(ItemInstanceUI _ItemUI)
    {
        txtName.text = _ItemUI.data.name;
        Icon.sprite = _ItemUI.data.Icon;
        txtDescription.text = itemUI.data.GetDescription();
        txtLVL.text = _ItemUI.GetLevel();
        var dic = itemUI.GetInfo();
        // Info.text = itemUI.data.GetInfo();
        for (int i = 0; i < dic.Count; i++)
        {
            Debug.Log("Key is " + dic[i].Key + " value is " + dic[i].Value);
            Infos[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = dic[i].Value;
            Sprite sprite = GetSprite(dic[i].Key);
            Infos[i].transform.GetComponentInChildren<Image>().sprite = sprite;
            Infos[i].gameObject.SetActive(true);
        }
    }

    private Sprite GetSprite(string key)
    {
        // Debug.Log("key is " + key);
        Sprite restult = IconPair.Where(x => x.key == key).First().value;
        if (restult == null)
        {
            Debug.LogError("Not Found");
        }
        return restult;
    }
}
