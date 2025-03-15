using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

[CreateAssetMenu(fileName = "SKillSO", menuName = "Skill/SKillSO")]
public class SkillSO : ScriptableObject
{
    public Sprite Sprite;
    [TextArea] public string Text;
    public SkillSO nextLevel;
    public KeyValuePair<Character, BaseSkill> Weilders;
    public BaseSkill Prefab;
    public virtual void Equip()
    {
        var InsObj = Instantiate(Prefab, Z.Player.transform.position, Quaternion.identity, Z.Player.transform);
        InsObj.Equip();
    }

    public virtual void UnEquip()
    {
        throw new NotImplementedException();
    }
}
