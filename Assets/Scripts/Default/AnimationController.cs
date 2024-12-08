using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : BaseAnim
{

    public AnimationState animationState;
    public Transform LookTarget;

    private void Update()
    {
        // if (LookTarget)
        // {

        // }
    }
    public void SetXY(float x, float y)
    {
        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
    }
    public void RightHandAttack(bool val)
    {
        // animator.SetBool("attack", val);
        if (val)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }

    public void Set8WayLayerWeight(bool value)
    {
        // if (value)
        // {
        //     animator.SetLayerWeight(0, 1);
        //     animator.SetLayerWeight(1, 0);
        // }
        // else
        // {
        //     animator.SetLayerWeight(0, 0);
        //     animator.SetLayerWeight(1, 1);
        // }
    }

    internal void SetHandSpeed(float v)
    {
        animator.SetFloat("handSpeed", v);
    }
}
public enum AnimationState
{
    Other, Idle, Walk, Run, Jump, Climb, Fall, Kick, Slide, HandJump
}
