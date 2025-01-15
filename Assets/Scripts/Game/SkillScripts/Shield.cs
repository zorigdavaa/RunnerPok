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
        Z.Player.AddToSkill(this);
        var Shied = Instantiate(ShieldObj, Z.Player.transform.position, Quaternion.identity, Z.Player.transform);
        Shied.OnDestroyEvent += Logic;

        // Physics.IgnoreCollision(Shied.GetComponent<Collider>())
    }
    public override void Logic(object sender, object e)
    {
        UnEquip();
        Destroy((GameObject)sender);
    }
    public override void UnEquip()
    {
        Z.Player.RemoveSkill(this);
    }
}
