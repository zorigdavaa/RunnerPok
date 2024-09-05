using System;
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
    public bool ControlAble;
    public ZControlType ControlType = ZControlType.None;
    Camera cam;
    Plane ControlRaycastPlane;
    private void Start()
    {
        groundLayer = LayerMask.GetMask("Road");
        childModel = transform.GetChild(0);
        if (playerParent == null)
        {
            playerParent = transform.parent;
        }
        cam = Camera.main;
        ControlRaycastPlane = new Plane(Vector3.up, Vector3.zero);
    }
    public void SetControlAble(bool value)
    {
        ControlAble = value;
    }
    public void SetControlType(ZControlType value)
    {
        ControlType = value;
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
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.13f, groundLayer);
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
        if (ControlType == ZControlType.TwoSide)
        {
            // OldController();
            ViewPortControl();
            // RaycastControl();
        }
        else if (ControlType == ZControlType.FourSide)
        {
            if (IsClick)
            {
                Vector3 TargetLocalPos = Vector3.zero;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (ControlRaycastPlane.Raycast(ray, out float enter))
                {
                    //Get the point that is clicked
                    Vector3 hitPoint = ray.GetPoint(enter);

                    //Move your cube GameObject to the point where you clicked
                    TargetLocalPos = transform.parent.InverseTransformPoint(hitPoint);
                    TargetLocalPos.x = Mathf.Clamp(TargetLocalPos.x, -5, 5);
                    TargetLocalPos.z = Mathf.Clamp(TargetLocalPos.z, -1, 8);
                }
                transform.localPosition = Vector3.Lerp(transform.localPosition, TargetLocalPos, 5 * Time.fixedDeltaTime);
            }
        }
    }
    private void RaycastControl()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); // Default forward rotation

        if (IsClick)
        {
            Vector3 TargetPos = Vector3.zero;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (ControlRaycastPlane.Raycast(ray, out float enter))
            {
                //Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);

                //Move your cube GameObject to the point where you clicked
                // TargetPos = transform.InverseTransformPoint(hitPoint);
                TargetPos = new Vector3(hitPoint.x, transform.position.y, transform.position.z); ;
                TargetPos.x = Mathf.Clamp(TargetPos.x, -5, 5);
            }
            transform.position = Vector3.Lerp(transform.position, TargetPos, 5 * Time.fixedDeltaTime);

            // Check for significant movement to determine the rotation
            if (Mathf.Abs(transform.position.x - TargetPos.x) > 0.5f)
            {
                float directionSign = Mathf.Sign(TargetPos.x - transform.localPosition.x);
                Vector3 moveDirection = new Vector3(directionSign, 0f, 1f).normalized;
                targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); // Rotate towards movement direction
            }
        }

        // Apply the target rotation smoothly in all cases
        childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
    }

    private void OldController()
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
    private void ViewPortControl()
    {
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); // Default forward rotation

        if (IsClick)
        {
            // Convert mouse position to viewport position
            Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
            float inverse = Mathf.InverseLerp(0.1f, 0.9f, viewPortPos.x);
            // Calculate new X position within the bounds (-5 to 5)
            float newPositionX = Mathf.Lerp(-5f, 5f, inverse);
            Vector3 targetPos = new Vector3(newPositionX, transform.localPosition.y, transform.localPosition.z);

            // Smoothly move the player to the target position
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.125f);

            // Check for significant movement to determine the rotation
            if (Mathf.Abs(transform.localPosition.x - newPositionX) > 0.5f)
            {
                float directionSign = Mathf.Sign(newPositionX - transform.localPosition.x);
                Vector3 moveDirection = new Vector3(directionSign, 0f, 1f).normalized;
                targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); // Rotate towards movement direction
            }
        }

        // Apply the target rotation smoothly in all cases
        childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
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

    internal void ChildModelRotZero()
    {
        childModel.transform.rotation = Quaternion.identity;
    }
}
