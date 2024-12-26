using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ShurForward : Shuriken
{

    public override void ShurikenBehavior()
    {
        Graphics.Rotate(0, 360 * Time.deltaTime, 0);
        if (Z.Player.GetState() == PlayerState.Fight)
        {

            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition += transform.forward * speed * 3 * Time.deltaTime;
            // Pool.Release(this);
        }
        // transform.localPosition += Vector3.right * SideMovement * Time.deltaTime;
        if (RightAcc < Mathf.Abs(SideMovement * 2))
        {
            RightAcc += Time.deltaTime;
        }
        transform.localPosition += Vector3.right * SideMovement * RightAcc * Time.deltaTime;
    }
}
