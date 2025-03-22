using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class CorrectedShuriken : Shuriken
{

    public float SideMovement;
    protected float RightAcc = 0;
    public override void GotoPool()
    {
        base.GotoPool();
        SideMovement = 0;
        RightAcc = 0;
    }

    public override void ShurikenBehavior()
    {
        Graphics.Rotate(0, 360 * Time.deltaTime, 0);
        if (Z.Player.GetState() == PlayerState.Fight)
        {

            transform.localPosition += Vector3.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition += Vector3.forward * speed * 3 * Time.deltaTime;
            // Pool.Release(this);
        }
        // transform.localPosition += Vector3.right * SideMovement * Time.deltaTime;
        if (RightAcc < Mathf.Abs(SideMovement * 2))
        {
            RightAcc += Time.deltaTime;
        }
        transform.localPosition += Vector3.right * SideMovement * RightAcc * Time.deltaTime;
    }
    public override void OnShoot(Player player)
    {
        if (Physics.SphereCast(player.transform.position + Vector3.up, 2f, player.transform.forward, out RaycastHit hit, 20, 1 << 6))
        {
            Vector3 point = new Vector3(transform.position.x, hit.transform.position.y, hit.transform.position.z);
            float distance = Vector3.Distance(point, hit.transform.position);

            Vector3 dirToNearest = transform.position - hit.transform.position;
            float half = distance * 0.7f;
            if (Vector3.Dot(dirToNearest, transform.right) > 0) // right

            {
                SideMovement = -half;
            }
            else
            {
                SideMovement = half;
            }
        }
    }
}
