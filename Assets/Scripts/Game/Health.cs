using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Health : BaseCollectAble
{
    public override void GotoPosAndAdd()
    {
        StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return null;
            shouldRot = false;
            transform.SetParent(null);
            float t = 0;
            float time = 0;
            float duration = 1f;
            Vector3 initpos = transform.position;
            // Vector3 initialPosition = transform.position;
            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                transform.position = Vector3.Lerp(initpos, Z.Player.transform.position, t);
                yield return null;
            }
            Z.Player.Stats.Health += Amount;
            // Debug.Log("Healed");
            Destroy(gameObject);
        }
    }
    public override void BenefitPLayer()
    {
        Z.Player.Stats.Health += Amount;
    }
}
