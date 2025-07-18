using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;
using ZPackage;

public class BaseSkill : MonoBehaviour
{
    public bool UseAble = false;
    public Countdown CoolDown;
    public SkillSO SO;
    public virtual void OnEquipped()
    {
        // Z.Player.AddToSkill(this);
    }
    public virtual void OnUnEquip()
    {
        // Z.Player.RemoveSkill(this);
    }

    public void SetSO(SkillSO SO)
    {
        this.SO = SO;
    }

    public virtual void Use(object sender, object e)
    {
        Debug.LogError("BaseSkill Not Implemented");
    }
}
