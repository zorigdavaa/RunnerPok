using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

[CreateAssetMenu(fileName = "Shield", menuName = "Skill/Shield")]
public class Shield : BaseSkill
{
    [SerializeField] ShieldObject ShieldObj;
    public override void Equip()
    {
        Instantiate(ShieldObj, Z.Player.transform.position, Quaternion.identity, Z.Player.transform);
    }
    public override void Logic(object sender, object e)
    {

    }
}
