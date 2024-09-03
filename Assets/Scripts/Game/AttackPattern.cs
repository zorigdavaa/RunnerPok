using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ForwardAndAttack", menuName = "Patter/Simple")]
public class AttackPattern : BaseAttackPattern
{
    public List<GameObject> ProjectilePfs;
    public int AttackCount = 6;
    public float animationWaitTimer = 0.2f;
    public float WaitTimer = 0.2f;
    // Animal animal;

    public void AttackProjectile(Animal animal)
    {
        GameObject pf = ProjectilePfs[Random.Range(0, ProjectilePfs.Count)];
        GameObject inSob = Instantiate(pf, animal.transform.position + Vector3.up, animal.transform.rotation, animal.transform.parent);
        Destroy(inSob, 10);
    }
    public IEnumerator Pattern(Animal animal)
    {

        Vector3 attackPos = animal.transform.localPosition + Vector3.back * 10 + Vector3.right * Random.Range(-3, 3);
        float t = 0;
        float duration = 0.5f;
        float time = 0;
        Vector3 initPos = animal.transform.localPosition;
        if (Mathf.Abs(attackPos.x) > 6)
        {
            attackPos.x = Mathf.Clamp(attackPos.x, -6, 6);
        }
        while (time < duration)
        {
            time += Time.deltaTime;
            t = time / duration;
            animal.transform.localPosition = Vector3.Lerp(initPos, attackPos, t);
            yield return null;
        }
        for (int i = 0; i < AttackCount; i++)
        {
            animal.AttackAnimation();
            yield return new WaitForSeconds(animationWaitTimer);
            AttackProjectile(animal);
            yield return new WaitForSeconds(WaitTimer);
        }

        time = 0;
        duration = 1f;
        while (time < duration)
        {
            time += Time.deltaTime;
            t = time / duration;
            animal.transform.localPosition = Vector3.Lerp(attackPos, initPos, t);
            yield return null;
        }

    }
}
