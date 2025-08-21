using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISlot : MonoBehaviour, IDropHandler, IPointerUpHandler, IPointerDownHandler
{
    ItemInstanceUI Item;
    public ItemInstanceUI GetItem() => Item;
    public WhereSlot Where;
    // public bool WearSlot = false;
    public Image WearSlotImage;
    public Sprite[] SlotSprites;
    public EventHandler<ItemInstanceUI> OnItemChanged;
    public bool isUnequipSlot = false;
    public TextMeshProUGUI priceText;
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

    internal void AddItem(ItemInstanceUI baseItemUI)
    {
        gameObject.SetActive(true);
        if (baseItemUI.currentSlot != null)
        {
            baseItemUI.currentSlot.RemoveItem();
        }
        Item = baseItemUI;
        GetItem().transform.position = transform.position;
        GetItem().transform.SetParent(transform);
        GetItem().currentSlot = this;
        priceText.text = baseItemUI.Price.ToString();
        priceText.gameObject.SetActive(true);
        // if (WearSlot)
        // {
        // }
        // else
        // {
        // }
        if (isUnequipSlot)
        {
            GetItem().UnEquipItem();
            Where = GetItem().data.Where;
            SetImageUsingSlot();
        }
        else
        {
            GetItem().EquipItem();
        }
        OnItemChanged?.Invoke(this, GetItem());
    }
    internal void RemoveItem()
    {
        Item = null;
        priceText.gameObject.SetActive(false);
        OnItemChanged?.Invoke(this, GetItem());
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
        if (GetItem() != null)
        {
            ItemInfoCanvas.Instance.ShowInfoOf(GetItem());
        }
    }
}
