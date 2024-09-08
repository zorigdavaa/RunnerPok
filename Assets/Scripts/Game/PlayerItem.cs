using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerItem : MonoBehaviour, IUpgradeAble, ISaveAble
{
    public string itemName;
    public int Damage;

    public void RetrieveData()
    {
        Damage = PlayerPrefs.GetInt(itemName, 5);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(itemName, Damage);
    }

    public void Upgrade()
    {
        Damage = (int)(Damage * 1.1f);
    }

}
