using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Coin : BaseCollectAble
{
    public override void BenefitPLayer()
    {
        Z.GM.Coin++;
    }
}
