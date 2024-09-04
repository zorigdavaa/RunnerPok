using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Patter/Simple")]
public class AttackSimple : BaseAttackPattern
{
    public List<GameObject> ProjectilePfs;
    public int AttackCount = 6;
    public float animationWaitTimer = 0.2f;
    public float WaitTimer = 0.2f;
    public float Cooldown = 0.2f;
    public override void AttackProjectile(Animal animal)
    {
        GameObject pf = ProjectilePfs[Random.Range(0, ProjectilePfs.Count)];
        GameObject inSob = Instantiate(pf, animal.transform.position + Vector3.up, animal.transform.rotation, animal.transform.parent);
        Destroy(inSob, 10);
    }

    public override float GetCoolDown()
    {
        return Cooldown;
    }

    public override IEnumerator Pattern(Animal animal)
    {
        Vector3 initPos = animal.transform.localPosition;
        for (int i = 0; i < AttackCount; i++)
        {
            animal.AttackAnimation();
            yield return new WaitForSeconds(animationWaitTimer);
            AttackProjectile(animal);
            yield return new WaitForSeconds(WaitTimer);
        }
    }
}
