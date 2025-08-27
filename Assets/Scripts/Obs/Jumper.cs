using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZPackage.Helper;

public class Jumper : MonoBehaviour, ICollisionAction
{
    [SerializeField] Vector3 JumperForce;
    // public Tile CurrentTile;
    public Transform nextJumper;
    public Transform NextJumper
    {
        get { return nextJumper; }
        set
        {
            nextJumper = value;
            NextJumpTarget = value.position;
        }
    }

    public Vector3 NextJumpTarget;
    [SerializeField] Animation anim;
    public Transform pointer;
    public bool Test;
    [SerializeField] float jumpAng;
    public void Start()
    {
        if (NextJumper)
        {
            // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumper.transform.position, 60f);
            NextJumpTarget = NextJumper.position; ;
            // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumpTarget, 45f);
        }
        // else
        // {
        //     if (CurrentTile)
        //     {

        //         CurrentTile.OnNextTileSet += OnNextTileSet;
        //     }
        // }
        Direction();
    }

    private void Direction()
    {
        if (JumperForce.z > 0)
        {
            pointer.LookAt(pointer.position + Vector3.forward);
        }
        else
        {
            pointer.LookAt(pointer.position - Vector3.forward);
        }
    }

    // private void OnNextTileSet(object sender, EventArgs e)
    // {
    //     if (!NextJumper)
    //     {
    //         Tile nextTile = (Tile)sender;
    //         // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, nextTile.start.position, 60f);
    //         NextJumpTarget = nextTile.start.position + Vector3.forward * 1f;
    //         // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumpTarget, 60f);
    //         // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumpTarget, 45f);
    //         CalcForce(transform.position);
    //         Direction();
    //     }
    // }
    // public void OnTriggerEnter(Collider other)
    // {
    //     if (Test && other.GetComponent<Rigidbody>())
    //     {
    //         other.GetComponent<Rigidbody>().velocity = JumperForce;
    //     }
    // }

    public void CollisionAction(Character character)
    {

        if (character is Player player)
        {
            // player.Movement.UseParentedMovement(false);
            player.Movement.StopSlide();
            anim.Play();
            CalcForce(character.transform.position);
            character.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            character.GetComponent<Rigidbody>().linearVelocity = JumperForce;
        }

        // print(character.GetComponent<Rigidbody>().velocity);
        // Debug.Break();
        // character.GetComponent<Rigidbody>().AddForce(Vector3.up * 400);
    }

    private void CalcForce(Vector3 From)
    {
        // if (!NextJumper.gameObject.GetComponent<Jumper>())
        // {
        //     NextJumpTarget += Vector3.forward * 2;
        // }
        NextJumpTarget.x = transform.position.x;
        // float ydiff = NextJumper.position.y - transform.position.y;
        // jumpAng = Mathf.Lerp(40, 60, Mathf.InverseLerp(0, 20, ydiff));
        jumpAng = PhysicsHelper.CalculateLaunchAngle(From, NextJumpTarget);
        jumpAng = Mathf.Clamp(jumpAng, 30, 75);
        if (jumpAng > 31)
        {
            jumpAng += 10;
        }
        JumperForce = PhysicsHelper.CalcBallisticVelocityVectorNew(From, NextJumpTarget, jumpAng);
        // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(From, NextJumpTarget, 70f);
    }
}
