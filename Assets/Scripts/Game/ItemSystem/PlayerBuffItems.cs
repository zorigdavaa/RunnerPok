using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerBuffItems : MonoBehaviour
{
    public static PlayerBuffItems Instance;
    [SerializeField] List<UISlot> equipSlots;
    [SerializeField] List<UISlot> unEquipslots;
    [SerializeField] List<ItemData> buffItemDatas;
    [SerializeField] ItemInfoCanvas itemInfoCanvas;
    void Awake()
    {
        Instance = this;
        itemInfoCanvas.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {
        RetrieveData();
        // for (int i = 0; i < buffItemDatas.Count; i++)
        // {
        //     BaseItemUI insObj = Instantiate(buffItemDatas[i].pfUI, transform.position, Quaternion.identity);
        //     unEquipslots[i].AddItem(insObj);
        // }
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
        // PlayerPrefZ.SetData("buffData", equipSlots);
        SlotSave SavingData = new SlotSave();
        for (int i = 0; i < equipSlots.Count; i++)
        {
            if (equipSlots[i].Item != null)
            {
                SavingData.EquipedNames.Add(equipSlots[i].Item.data.name);
            }
            else
            {
                SavingData.EquipedNames.Add(null);
            }
        }
        for (int i = 0; i < unEquipslots.Count; i++)
        {
            if (unEquipslots[i].Item != null)
            {
                SavingData.UnEquipedNames.Add(unEquipslots[i].Item.data.name);
            }
            else
            {
                SavingData.UnEquipedNames.Add(null);
            }
        }
        PlayerPrefZ.SetData("equipedData", SavingData);
        Debug.Log(SavingData.EquipedNames.Count);
    }
    public void RetrieveData()
    {
        SlotSave defaultOne = new SlotSave();
        defaultOne.EquipedNames.Add(buffItemDatas[0].itemName);
        // defaultOne.EquipedNames.Add(null);
        // defaultOne.EquipedNames.Add(null);
        // defaultOne.EquipedNames.Add(null);
        // defaultOne.EquipedNames.Add(null);

        defaultOne.UnEquipedNames.Add(buffItemDatas[1].itemName);
        defaultOne.UnEquipedNames.Add(buffItemDatas[2].itemName);

        var saved = PlayerPrefZ.GetData("equipedData", defaultOne);
        Debug.Log(saved.EquipedNames.Count);
        if (saved.UnEquipedNames.Count > 0)
        {
            for (int i = 0; i < saved.UnEquipedNames.Count; i++)
            {
                if (saved.UnEquipedNames[i] == null)
                {
                    continue;
                }
                ItemData data = buffItemDatas.Where(x => x.itemName == saved.UnEquipedNames[i]).FirstOrDefault();
                if (data)
                {
                    BaseItemUI insObj = Instantiate(buffItemDatas[i].pfUI, transform.position, Quaternion.identity, transform);
                    insObj.transform.localScale = Vector3.one;
                    unEquipslots[i].AddItem(insObj);
                }
            }
        }
        if (saved.EquipedNames.Count > 0)
        {
            for (int i = 0; i < saved.EquipedNames.Count; i++)
            {
                if (saved.EquipedNames[i] == null)
                {
                    continue;
                }
                ItemData data = buffItemDatas.Where(x => x.itemName == saved.EquipedNames[i]).FirstOrDefault();
                if (data)
                {
                    BaseItemUI insObj = Instantiate(buffItemDatas[i].pfUI, transform.position, Quaternion.identity, transform);
                    insObj.transform.localScale = Vector3.one;
                    equipSlots[i].AddItem(insObj);
                }

            }
        }

    }

    internal UISlot GetByTypeFromEquiped(WhereSlot where)
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
}
public enum WhereSlot
{
    Any, Hand, Head, OtherHand, Chest, Foot
}
