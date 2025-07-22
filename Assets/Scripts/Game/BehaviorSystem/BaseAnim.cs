using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnim : MonoBehaviour
{
    public Animator animator;
    const string animIdle = "Idle";
    public string currentAnimation;
    string beforeAmimation = animIdle;

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
    public void ChangeAnimation(string animStr)
    {
        if (currentAnimation != animStr)
        {
            animator.CrossFade(animStr, 0f, 0);
            beforeAmimation = currentAnimation;
            currentAnimation = animStr;
            // transitionToIdle = false;
        }
    }

    internal void VelY(float y)
    {
        animator.SetFloat("velY", y);
    }

    internal void Slide(bool val)
    {
        animator.SetBool("isSliding", val);
    }
}
