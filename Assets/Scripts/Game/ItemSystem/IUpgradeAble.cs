using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeAble
{
    public void Upgrade();
}
public interface ISaveAble
{
    public void SaveData();
    public void RetrieveData();
}