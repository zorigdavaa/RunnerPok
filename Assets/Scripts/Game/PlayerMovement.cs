using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MovementForgeRun
{

    public float sideSpeed = 5f; // Speed at which the player moves left and right

    public Transform groundCheck; // Transform representing a point at the bottom of the player to check for ground
    public LayerMask groundLayer; // Layer mask for ground objects
    public float minXLimit = -5f; // Minimum X position limit
    public float maxXLimit = 5f; // Maximum X position limit
    public float rotSpeed = 10f; // Maximum X position limit
    bool ParentedMove = true;
    Transform childModel;
    private void Start()
    {
        groundLayer = LayerMask.GetMask("Road");
        childModel = transform.GetChild(0);
        if (playerParent == null)
        {
            playerParent = transform.parent;
        }
    }
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Q))
    //     {
    //         UseParentedMovement(true);
    //     }
    //     else if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         UseParentedMovement(false);
    //     }
    // }

    // void FixedUpdate()
    // {
    //     PlayerControl();
    //     // }
    // }

    public void PlayerControl()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.12f, groundLayer);
        if (isGrounded)
        {
            animController.Jump(false);
            // UseParentedMovement(true);
        }
        else
        {
            animController.Jump(true);
            animController.VelY(rb.velocity.y);
            // if (rb.velocity.y < 0f)
            // {
            //     rb.velocity += Vector3.down * 0.8f;
            // }
            // rb.velocity += Vector3.down * 1.2f;
        }

        // Move the player forward
        // transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        if (ParentedMove)
        {
            playerParent.Translate(Vector3.forward * Speed * Time.deltaTime);
            Vector3 vel = rb.velocity;
            vel.z = 0;
            rb.velocity = vel;
        }
        else
        {
            if (isGrounded && rb.velocity.y < 1)
            {
                Vector3 vel = rb.velocity;
                if (vel.z < Speed)
                {
                    // vel.z = Speed;
                    vel.z = Mathf.Lerp(vel.z, Speed, 0.2f);
                }
                else
                {
                    vel.z = Mathf.MoveTowards(vel.z, Speed, 1 * Time.fixedDeltaTime);
                }
                rb.velocity = vel;
            }

        }
        // if (IsPlaying)
        // {
        if (ControlAble)
        {

            float horizontalInput = 0;
            // Move the player left and right
            if (IsClick)
            {
                horizontalInput = Input.GetAxisRaw("Mouse X");
            }
            // print(horizontalInput);
            float newPositionX = transform.localPosition.x + (horizontalInput * sideSpeed * Time.deltaTime);
            newPositionX = Mathf.Clamp(newPositionX, minXLimit, maxXLimit);
            Vector3 targetPos = new Vector3(newPositionX, transform.localPosition.y, transform.localPosition.z);
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.35f);
            if (Mathf.Abs(horizontalInput) > 0)
            {
                float rot = Mathf.Sign(horizontalInput);
                // print(rot);
                Vector3 moveDirection = new Vector3(rot, 0f, 1).normalized;
                // print(moveDirection);
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
            }
            else
            {
                childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, Quaternion.Euler(Vector3.forward), Time.deltaTime * rotSpeed);
            }
        }
    }

    public override void UseParentedMovement(bool val)
    {
        if (ParentedMove != val)
        {
            if (val)
            {
                Vector3 toBeParentPos = new Vector3(0, 0, transform.position.z);
                playerParent.transform.position = toBeParentPos;
                transform.SetParent(playerParent);
                Vector3 tobeLocaPos = transform.localPosition;
                tobeLocaPos.z = 0;
                transform.localPosition = tobeLocaPos;
            }
            else
            {
                transform.SetParent(null);
            }
            ParentedMove = val;
        }
        // print(rb.velocity);

    }
}
