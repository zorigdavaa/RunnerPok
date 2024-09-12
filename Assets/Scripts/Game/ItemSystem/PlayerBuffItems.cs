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
        SlotSave EquipedSaveSlot = new SlotSave();
        for (int i = 0; i < equipSlots.Count; i++)
        {
            if (equipSlots[i].Item != null)
            {
                EquipedSaveSlot.saveNames.Add(equipSlots[i].Item.data.name);
            }
            else
            {
                EquipedSaveSlot.saveNames.Add(null);
            }
        }
        PlayerPrefZ.SetData("equipedData", EquipedSaveSlot);
        Debug.Log(EquipedSaveSlot.saveNames.Count);
    }
    public void RetrieveData()
    {
        SlotSave defaultOne = new SlotSave();
        defaultOne.saveNames.Add(buffItemDatas[0].itemName);
        defaultOne.saveNames.Add(null);
        defaultOne.saveNames.Add(null);
        defaultOne.saveNames.Add(null);
        defaultOne.saveNames.Add(null);

        var saved = PlayerPrefZ.GetData("equipedData", defaultOne);
        Debug.Log(saved.saveNames.Count);
        if (saved.saveNames.Count > 0)
        {
            for (int i = 0; i < saved.saveNames.Count; i++)
            {
                if (saved.saveNames[i] == null)
                {
                    continue;
                }
                ItemData data = buffItemDatas.Where(x => x.itemName == saved.saveNames[i]).FirstOrDefault();
                if (data)
                {
                    BaseItemUI insObj = Instantiate(buffItemDatas[i].pfUI, transform.position, Quaternion.identity, transform);
                    insObj.transform.localScale = Vector3.one;
                    unEquipslots[i].AddItem(insObj);
                }

            }
        }
    }

    internal UISlot GetByTypeFromEquiped(PlayerItemSlot where)
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
public enum PlayerItemSlot
{
    Any, Hand, Head, OtherHand, Chest, Foot
}
