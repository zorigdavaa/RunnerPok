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
    public int skillLevel = 1;
    public KeyValuePair<Character, BaseSkill> Weilders;
    public BaseSkill Prefab;
    public virtual void Equip()
    {

    }

    public virtual void UnEquip()
    {
        throw new NotImplementedException();
    }
}
