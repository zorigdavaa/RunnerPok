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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
public enum PlayerItemSlot
{
    Any, Hand,Head,OtherHand,Chest,Foot
}
