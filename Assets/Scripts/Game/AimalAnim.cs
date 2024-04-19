using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAnim : BaseAnim
{
    public override void Attack()
    {
        animator.SetTrigger("attack1");
    }
    public void Attack2()
    {
        animator.SetTrigger("attack2");
    }
    public void Victory()
    {
        animator.SetTrigger("victory");
    }
    public void Taunt()
    {
        animator.SetTrigger("taunt");
    }
    public void Dizzy()
    {
        animator.SetTrigger("dizzy");
    }
    public override void Die()
    {
        animator.SetTrigger("death");
    }
}
