using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IDropHandler, IPointerUpHandler, IPointerDownHandler
{
    public BaseItemUI Item;
    public WhereSlot Where;
    public bool WearSlot = false;
    public Image WearSlotImage;
    public Sprite[] SlotSprites;
    public EventHandler<BaseItemUI> OnItemChanged;
    public bool isUnequipSlot = false;
    // Start is called before the first frame update
    void Start()
    {
        SetImageUsingSlot();
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
        gameObject.SetActive(true);
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
        if (isUnequipSlot)
        {
            Where = Item.data.Where;
            SetImageUsingSlot();
        }
        OnItemChanged?.Invoke(this, Item);
    }
    internal void RemoveItem()
    {
        Item = null;
        OnItemChanged?.Invoke(this, Item);
        // transform.SetSiblingIndex(transform.parent.childCount - 1);
        // gameObject.SetActive(false);
    }
    [ContextMenu("Set Icon")]
    public void SetImageUsingSlot()
    {
        WearSlotImage.sprite = SlotSprites[(int)Where];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item != null)
        {
            ItemInfoCanvas.Instance.ShowInfoOf(Item);
        }
    }
}
