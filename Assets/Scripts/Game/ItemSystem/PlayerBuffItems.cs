using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBuffItems : MonoBehaviour
{
    private static PlayerBuffItems _instacne;
    public static PlayerBuffItems Instance
    {
        get
        {
            if (_instacne == null)
            {
                _instacne = FindObjectOfType<PlayerBuffItems>(true);
            }
            return _instacne;
        }
    }

    public static Action<UISlot, BaseItemUI> OnSlotChanged;

    [SerializeField] List<UISlot> equipSlots;
    [SerializeField] List<UISlot> unEquipslots;
    [SerializeField] List<BaseItemData> buffItemDatas;
    [SerializeField] ItemInfoCanvas itemInfoCanvas;
    public GameObject UIPF;
    void Awake()
    {
        itemInfoCanvas.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        // RetrieveData();
        // for (int i = 0; i < buffItemDatas.Count; i++)
        // {
        //     BaseItemUI insObj = Instantiate(buffItemDatas[i].pfUI, transform.position, Quaternion.identity);
        //     unEquipslots[i].AddItem(insObj);
        // }
        foreach (var item in equipSlots)
        {
            item.OnItemChanged += OnSlotObjChanged;
            item.isUnequipSlot = false;
        }
        foreach (var item in unEquipslots)
        {
            item.OnItemChanged += OnSlotObjChanged;
            item.isUnequipSlot = true;
        }
    }

    private void OnSlotObjChanged(object sender, BaseItemUI e)
    {
        var casted = (UISlot)sender;
        if (casted.isUnequipSlot && e == null)
        {
            casted.gameObject.SetActive(false);
        }
        // else if (!casted.isUnequipSlot && e)
        // {
        // }
        OnSlotChanged?.Invoke(casted, e);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveData();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            RetrieveData();
        }
    }
    public void SaveData()
    {
        SlotSave SavingData = new SlotSave();
        for (int i = 0; i < equipSlots.Count; i++)
        {
            if (equipSlots[i].Item != null)
            {
                SavingData.EquipDataNew.Add(new ItemSaveData(equipSlots[i].Item.ID, equipSlots[i].Item.data.itemName));
            }
            else
            {
                SavingData.EquipDataNew.Add(new ItemSaveData());
            }
        }
        for (int i = 0; i < unEquipslots.Count; i++)
        {
            if (unEquipslots[i].Item != null)
            {
                SavingData.UneqiupDataNew.Add(new ItemSaveData(unEquipslots[i].Item.ID, unEquipslots[i].Item.data.itemName));
            }
            else
            {
                SavingData.UneqiupDataNew.Add(new ItemSaveData());
            }
        }
        PlayerPrefZ.SetData("equipedData", SavingData);
    }
    public void RetrieveData()
    {
        SlotSave defaultOne = new SlotSave();

        // defaultOne.EquipDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[1].itemName));
        defaultOne.EquipDataNew.Add(new ItemSaveData());
        defaultOne.EquipDataNew.Add(new ItemSaveData());
        defaultOne.EquipDataNew.Add(new ItemSaveData());
        defaultOne.EquipDataNew.Add(new ItemSaveData());
        defaultOne.EquipDataNew.Add(new ItemSaveData());

        defaultOne.UneqiupDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[2].itemName));
        defaultOne.UneqiupDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[3].itemName));
        defaultOne.UneqiupDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[4].itemName));
        defaultOne.UneqiupDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[5].itemName));
        defaultOne.UneqiupDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[6].itemName));
        defaultOne.UneqiupDataNew.Add(new ItemSaveData(Guid.NewGuid().ToString(), buffItemDatas[7].itemName));

        var saved = PlayerPrefZ.GetData("equipedData", defaultOne);

        if (saved.UneqiupDataNew.Count > 0)
        {
            for (int i = 0; i < saved.UneqiupDataNew.Count; i++)
            {
                if (saved.UneqiupDataNew[i].ID == String.Empty)
                {
                    continue;
                }
                BaseItemData data = buffItemDatas.Where(x => x.itemName == saved.UneqiupDataNew[i].dataName).FirstOrDefault();
                if (data)
                {
                    // ItemInstance insObj = Instantiate(UIPF, transform.position, Quaternion.identity, transform);
                    GameObject insObj = Instantiate(UIPF, transform.position, Quaternion.identity, transform);
                    Type var = GetComponentType(data.Where);
                    ItemInstance addedComponent = (ItemInstance)insObj.AddComponent(var);
                    addedComponent.ID = saved.UneqiupDataNew[i].ID;
                    addedComponent.data = data;
                    // insObj.SetIcon(data.Icon);
                    insObj.transform.localScale = Vector3.one;
                    unEquipslots[i].AddItem(addedComponent);
                }
            }
        }
        if (saved.EquipDataNew.Count > 0)
        {
            for (int i = 0; i < saved.EquipDataNew.Count; i++)
            {
                if (saved.EquipDataNew[i].ID == String.Empty)
                {
                    continue;
                }
                BaseItemData data = buffItemDatas.Where(x => x.itemName == saved.EquipDataNew[i].dataName).FirstOrDefault();
                if (data)
                {
                    // Debug.Log("Insed equip " + data.name);
                    // ItemInstance insObj = Instantiate(UIPF, transform.position, Quaternion.identity, transform);
                    GameObject insObj = Instantiate(UIPF, transform.position, Quaternion.identity, transform);
                    Type var = GetComponentType(data.Where);
                    ItemInstance addedComponent = (ItemInstance)insObj.AddComponent(var);
                    addedComponent.ID = saved.EquipDataNew[i].ID;
                    addedComponent.data = data;
                    // insObj.SetIcon(data.Icon);
                    insObj.transform.localScale = Vector3.one;
                    UISlot sameTypeSlot = equipSlots.Where(x => x.Where == addedComponent.data.Where && x.Item == null).FirstOrDefault();
                    if (sameTypeSlot)
                    {
                        sameTypeSlot.AddItem(addedComponent);
                    }
                    else
                    {
                        Debug.Log("Already Worn That kind of Item");
                    }
                }
            }
        }

    }

    private Type GetComponentType(WhereSlot where)
    {
        switch (where)
        {
            case WhereSlot.Hand: return typeof(ItemInstance);
            case WhereSlot.OtherHand: return typeof(OffHandItemInstance);
            case WhereSlot.Foot: return typeof(FootItemInstance);
            case WhereSlot.Head: return typeof(HeadItemInstance);
            case WhereSlot.Chest: return typeof(ChestItemInstance);
            default: return typeof(ItemInstance);
        }
    }

    internal UISlot GetByTypeFromEquipedFree(WhereSlot where)
    {
        foreach (var slot in equipSlots)
        {
            if (slot.Where == where && slot.Item == null)
            {
                return slot;
            }
        }
        return null;
    }

    internal UISlot GetFreefromUnEquip()
    {
        foreach (var slot in unEquipslots)
        {
            if (slot.Item == null)
            {
                return slot;
            }
        }
        return null;
    }

    internal UISlot GetByTypeFromEquiped(WhereSlot where)
    {
        foreach (var slot in equipSlots)
        {
            if (slot.Where == where)
            {
                return slot;
            }
        }
        return null;
    }
}
public enum WhereSlot
{
    None, Hand, Head, OtherHand, Chest, Foot, Necklace
}
