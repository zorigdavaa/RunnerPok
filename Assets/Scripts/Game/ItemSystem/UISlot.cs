using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IDropHandler, IPointerUpHandler, IPointerDownHandler
{
    public BaseItemUI Item;
    public PlayerItemSlot Where;
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
        Item = baseItemUI;
        Item.transform.position = transform.position;
        Item.transform.SetParent(transform);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item != null)
        {
            ItemInfoCanvas.Instance.ShowInfoOf(Item);
        }
    }
}
