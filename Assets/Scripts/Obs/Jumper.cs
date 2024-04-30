using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage.Helper;

public class Jumper : MonoBehaviour, ICollisionAction
{
    [SerializeField] Vector3 JumperForce;
    public Tile CurrentTile;
    public Transform NextJumper;
    public Vector3 NextJumpTarget;
    [SerializeField] Animation anim;
    public Transform pointer;
    public void Start()
    {
        if (NextJumper)
        {
            // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumper.transform.position, 60f);
            NextJumpTarget = NextJumper.position;
            JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumpTarget, 45f);
        }
        else
        {
            if (CurrentTile)
            {

                CurrentTile.OnNextTileSet += OnNextTileSet;
            }
        }
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

    private void OnNextTileSet(object sender, EventArgs e)
    {
        if (!NextJumper)
        {
            Tile nextTile = (Tile)sender;
            // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, nextTile.start.position, 60f);
            NextJumpTarget = nextTile.start.position + Vector3.forward * 1f;
            // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumpTarget, 60f);
            JumperForce = PhysicsHelper.CalcBallisticVelocityVector(transform.position, NextJumpTarget, 45f);
            Direction();
        }
    }

    public void CollisionAction(Character character)
    {
        // character.GetComponent<Rigidbody>().velocity = Vector3.up * 10 + Vector3.forward * 10;
        if (character is Player player)
        {
            player.Movement.UseParentedMovement(false);
        }
        anim.Play();
        // JumperForce = PhysicsHelper.CalcBallisticVelocityVector(character.transform.position, NextJumpTarget, 60f);

        character.GetComponent<Rigidbody>().velocity = JumperForce;
        print(character.GetComponent<Rigidbody>().velocity);
        // Debug.Break();
        // character.GetComponent<Rigidbody>().AddForce(Vector3.up * 400);
    }
}
