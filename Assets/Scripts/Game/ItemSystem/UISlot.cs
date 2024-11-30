using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler, IPointerUpHandler, IPointerDownHandler
{
    public BaseItemUI Item;
    public WhereSlot Where;
    public bool WearSlot = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag)
        {
            eventData.pointerDrag.transform.position = transform.position;
            Debug.Log("Set");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // throw new System.NotImplementedException();
    }

    internal void AddItem(BaseItemUI baseItemUI)
    {
        if (baseItemUI.currentSlot != null)
        {
            baseItemUI.currentSlot.RemoveItem();
        }
        Item = baseItemUI;
        Item.transform.position = transform.position;
        Item.transform.SetParent(transform);
        Item.currentSlot = this;
        if (WearSlot)
        {
            Item.EquipItem();
        }
        else
        {
            Item.UnEquipItem();
        }

    }
    internal void RemoveItem()
    {
        Item = null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item != null)
        {
            ItemInfoCanvas.Instance.ShowInfoOf(Item);
        }
    }
}
