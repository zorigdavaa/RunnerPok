using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnim : MonoBehaviour
{
    public Animator animator;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    public virtual void Attack()
    {

    }

    public EventHandler OnAttackEvent;
    public void OnAttack()
    {
        OnAttackEvent?.Invoke(this, EventArgs.Empty);
    }
    internal void SetSpeed(float value)
    {
        animator.SetFloat("speed", value);
    }

    public virtual void Die()
    {
        animator.SetBool("death", true);
    }
    internal void Jump(bool value)
    {
        // animationState = AnimationState.Jump;
        // ResetJump();
        // animator.ResetTrigger("down");
        animator.SetBool("isJumping", value);
    }
}
