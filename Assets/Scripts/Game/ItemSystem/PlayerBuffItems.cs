using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffItems : MonoBehaviour
{
    public static PlayerBuffItems Instance;
    [SerializeField] List<UISlot> equipSlots;
    [SerializeField] List<UISlot> unEquipslots;
    [SerializeField] List<BaseItemUI> buffItemDatas;
    [SerializeField] ItemInfoCanvas itemInfoCanvas;
    void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        // RetrieveData();
        for (int i = 0; i < buffItemDatas.Count; i++)
        {
            BaseItemUI insObj = Instantiate(buffItemDatas[i], transform.position, Quaternion.identity);
            unEquipslots[i].AddItem(insObj);
        }
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
        PlayerPrefZ.SetData("buffData", equipSlots);
        Debug.Log(equipSlots.Count);
    }
    public void RetrieveData()
    {
        var saved = PlayerPrefZ.GetData("buffData", new List<UISlot>());
        Debug.Log(saved.Count);
        if (saved.Count > 0)
        {
            for (int i = 0; i < saved.Count; i++)
            {
                equipSlots[i].Where = saved[i].Where;
            }
        }
    }

    internal void GetByType(PlayerItemSlot where)
    {
        throw new NotImplementedException();
    }
}
public enum PlayerItemSlot
{
    Any, Hand, Head, OtherHand, Chest, Foot
}
