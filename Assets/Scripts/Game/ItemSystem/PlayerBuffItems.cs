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

    [SerializeField] List<UISlot> equipSlots;
    [SerializeField] List<UISlot> unEquipslots;
    [SerializeField] List<ItemData> buffItemDatas;
    [SerializeField] ItemInfoCanvas itemInfoCanvas;
    public ShurikenUI UIPF;
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
                SavingData.EquipedNames.Add(equipSlots[i].Item.data.itemName);
            }
            else
            {
                SavingData.EquipedNames.Add(String.Empty);
            }
        }
        for (int i = 0; i < unEquipslots.Count; i++)
        {
            if (unEquipslots[i].Item != null)
            {
                SavingData.UnEquipedNames.Add(unEquipslots[i].Item.data.itemName);
            }
            else
            {
                SavingData.UnEquipedNames.Add(String.Empty);
            }
        }
        foreach (var item in SavingData.EquipedNames)
        {
            Debug.Log("Save " + item);
        }
        PlayerPrefZ.SetData("equipedData", SavingData);
        Debug.Log(SavingData.EquipedNames.Count);
    }
    public void RetrieveData()
    {
        SlotSave defaultOne = new SlotSave();
        defaultOne.EquipedNames.Add(buffItemDatas[3].itemName);
        defaultOne.EquipedNames.Add(String.Empty);
        defaultOne.EquipedNames.Add(String.Empty);
        defaultOne.EquipedNames.Add(String.Empty);
        defaultOne.EquipedNames.Add(String.Empty);

        defaultOne.UnEquipedNames.Add(buffItemDatas[1].itemName);
        defaultOne.UnEquipedNames.Add(buffItemDatas[2].itemName);
        defaultOne.UnEquipedNames.Add(buffItemDatas[0].itemName);

        var saved = PlayerPrefZ.GetData("equipedData", defaultOne);

        // Debug.Log(saved.EquipedNames.Count);
        if (saved.UnEquipedNames.Count > 0)
        {
            for (int i = 0; i < saved.UnEquipedNames.Count; i++)
            {
                if (saved.UnEquipedNames[i] == String.Empty)
                {
                    // Debug.Log("Contunued uneqiop");
                    continue;
                }
                ItemData data = buffItemDatas.Where(x => x.itemName == saved.UnEquipedNames[i]).FirstOrDefault();
                if (data)
                {
                    // Debug.Log("Insed " + data.name);
                    // BaseItemUI insObj = Instantiate(data.pfUI, transform.position, Quaternion.identity, transform);
                    ShurikenUI insObj = Instantiate(UIPF, transform.position, Quaternion.identity, transform);
                    insObj.data = data;
                    insObj.SetIcon(data.Icon);
                    insObj.transform.localScale = Vector3.one;
                    unEquipslots[i].AddItem(insObj);
                }
            }
        }
        // foreach (var item in saved.EquipedNames)
        // {
        //     Debug.Log("Retrieve " + item);
        // }
        if (saved.EquipedNames.Count > 0)
        {
            for (int i = 0; i < saved.EquipedNames.Count; i++)
            {
                if (saved.EquipedNames[i] == String.Empty)
                {
                    // Debug.Log("Contunued equip");
                    continue;
                }
                ItemData data = buffItemDatas.Where(x => x.itemName == saved.EquipedNames[i]).FirstOrDefault();
                if (data)
                {
                    // Debug.Log("Insed equip " + data.name);
                    ShurikenUI insObj = Instantiate(UIPF, transform.position, Quaternion.identity, transform);
                    insObj.data = data;
                    insObj.SetIcon(data.Icon);
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
