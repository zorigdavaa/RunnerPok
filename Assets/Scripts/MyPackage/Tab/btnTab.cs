using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class btnTab : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    Tab tab;
    public Image BackGround;
    public Image Icon;
    public TMP_Text Text;

    public void OnPointerDown(PointerEventData eventData)
    {
        tab.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tab.OnTabExit(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        tab = transform.parent.parent.GetComponent<Tab>();
        tab.Subscribe(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tab.OnTabHover(this);
    }
}
