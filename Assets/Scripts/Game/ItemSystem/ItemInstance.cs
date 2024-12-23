using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInstance : BaseItemUI
{
    // Start is called before the first frame update
    void Start()
    {
        level = PlayerPrefs.GetInt(ID, 1);
    }


    // Update is called once per frame
    void Update()
    {

    }
    public override void Upgrade()
    {
        // if (level < data.AddDamage.Count)
        if (data.IsUpgradeAble(level))
        {
            level++;
            SaveData();
            Debug.Log("Upgraded " + level);
        }
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt(ID, level);
    }
    public void Equip()
    {

    }

    internal void SetIcon(Sprite icon)
    {
        GetComponent<Image>().sprite = icon;
    }
}
