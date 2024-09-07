using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tab : MonoBehaviour
{
    public List<btnTab> TabBtns;
    public List<GameObject> Tabs;
    btnTab currentSelectedTab;
    private void Start()
    {

    }
    public void Subscribe(btnTab button)
    {
        if (TabBtns == null)
        {
            TabBtns = new List<btnTab>();
        }
        TabBtns.Add(button);
        button.Text.gameObject.SetActive(false);
    }
    public void OnTabEnter(btnTab btn)
    {
        if (currentSelectedTab != btn)
        {
            if (currentSelectedTab != null)
            {
                int oldIndex = currentSelectedTab.transform.GetSiblingIndex();
                Tabs[oldIndex].gameObject.SetActive(false);
                currentSelectedTab.Icon.rectTransform.localPosition -= Vector3.up * 50;
                currentSelectedTab.Text.gameObject.SetActive(false);
            }

            currentSelectedTab = btn;
            int index = btn.transform.GetSiblingIndex();
            Tabs[index].gameObject.SetActive(true);
            currentSelectedTab.Icon.rectTransform.localPosition += Vector3.up * 50;
            currentSelectedTab.Text.gameObject.SetActive(true);
        }
        // btn.BackGround.color = Color.green;
    }
    public void OnTabExit(btnTab btn)
    {
        btn.BackGround.color = Color.white;
    }

    public void OnTabHover(btnTab btn)
    {
        if (btn != currentSelectedTab)
        {
            btn.BackGround.color = Color.grey;
        }
    }
}
