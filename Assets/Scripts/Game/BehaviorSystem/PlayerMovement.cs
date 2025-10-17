using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using ZPackage;
using UnityEngine.EventSystems;

public class PlayerMovement : MovementForgeRun
{

    public float sideSpeed = 5f; // Speed at which the player moves left and right
    Player Player;
    public Transform groundCheck; // Transform representing a point at the bottom of the player to check for ground
    public LayerMask groundLayer; // Layer mask for ground objects
    public float minXLimit = -8f; // Minimum X position limit
    public float maxXLimit = 8f; // Maximum X position limit
    public float rotSpeed = 10f; // Maximum X position limit
    bool ParentedMove = true;
    Transform childModel;
    public bool ControlAble;
    public ZControlType ControlType = ZControlType.None;
    public MovementState movementState;
    Camera cam;
    Plane ControlRaycastPlane;
    public bool addingForwardForece = false;
    private void Start()
    {
        Player = Z.Player;
        groundLayer = LayerMask.GetMask("Road");
        childModel = transform.GetChild(0);
        if (playerParent == null)
        {
            playerParent = transform.parent;
        }
        cam = Camera.main;
        ControlRaycastPlane = new Plane(Vector3.up, Vector3.zero);
        ClickAction.performed += Clicked;
    }

    //Performed is called when click and hold for 0.5 sec
    private void Clicked(InputAction.CallbackContext context)
    {
        // Debug.Log("Click Performed");
    }

    public void SetControlAble(bool value)
    {
        ControlAble = value;
    }
    public void SetControlType(ZControlType value)
    {
        ControlType = value;
        ChildModelRotZero();
        if (value == ZControlType.TwoSide)
        {
            ResetTargetX();
        }
    }
    public void SetMovementState(MovementState state)
    {
        movementState = state;
        // if (state == MovementState.Sliding)
        // {
        //     animController.Slide(true);
        // }
        // else
        // {
        //     animController.Slide(false);
        // }
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
        // isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
        isGrounded = IsGrounded();
        if (isGrounded)
        {
            animController.Jump(false);
            // UseParentedMovement(true);
        }
        else
        {
            animController.Jump(true);
            animController.VelY(rb.linearVelocity.y);
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
            Vector3 vel = rb.linearVelocity;
            vel.z = 0;
            rb.linearVelocity = vel;
        }
        else
        {
            // OldForward();
            Forward();

        }
        // if (IsPlaying)
        // {
        if (ControlType == ZControlType.TwoSide)
        {
            // OldController();
            // ViewPortControl();
            ViewPortControl2();
            // RaycastControl();
        }
        else if (ControlType == ZControlType.FourSide)
        {
            if (IsClick)
            {
                Vector3 TargetLocalPos = Vector3.zero;
                Ray ray = cam.ScreenPointToRay(Pointer.current.position.ReadValue());

                if (ControlRaycastPlane.Raycast(ray, out float enter))
                {
                    //Get the point that is clicked
                    Vector3 hitPoint = ray.GetPoint(enter);

                    //Move your cube GameObject to the point where you clicked
                    TargetLocalPos = transform.parent.InverseTransformPoint(hitPoint + Vector3.forward * 2);
                    TargetLocalPos.x = Mathf.Clamp(TargetLocalPos.x, minXLimit, maxXLimit);
                    TargetLocalPos.z = Mathf.Clamp(TargetLocalPos.z, -1, 10);
                }
                transform.localPosition = Vector3.Lerp(transform.localPosition, TargetLocalPos, 5 * Time.fixedDeltaTime);
                // befFrameMous = cam.ScreenToViewportPoint(Input.mousePosition);
            }
        }
    }
    private void Forward()
    {
        addingForwardForece = false;
        // if (isGrounded && rb.linearVelocity.y < 1)
        if (isGrounded && movementState != MovementState.Sliding)
        {
            Vector3 vel = rb.linearVelocity;

            Vector3 desiredVelocirt = vel.normalized * Speed;
            if (vel.magnitude < Speed)
            {
                // Step 1: Get the character's forward direction
                Vector3 flatForward = (transform.forward - Vector3.up * 0.1f).normalized;

                // Step 2: Project it onto the slope
                Vector3 groundNormal = GetGroundNormal();
                Vector3 slopeForward = Vector3.ProjectOnPlane(flatForward, groundNormal).normalized;

                // Step 3: Use that as your desired direction
                desiredVelocirt = slopeForward * Speed;
                vel = Vector3.Lerp(vel, desiredVelocirt, 0.2f);


            }
            else
            {
                vel = Vector3.MoveTowards(vel, desiredVelocirt, 1 * Time.fixedDeltaTime);
            }
            if (!rb.isKinematic)
            {
                rb.linearVelocity = vel;
            }
            addingForwardForece = true;
        }
        else if (!isGrounded && rb.linearVelocity.y < 0.5f)
        {
            rb.AddForce(Vector3.down * 10);
        }
    }

    private void OldForward()
    {
        // if (isGrounded && rb.linearVelocity.y < 1)
        if (isGrounded)
        {
            Vector3 vel = rb.linearVelocity;

            if (vel.z < Speed)
            {
                // vel.z = Speed;
                Vector3 groundNormal = GetGroundNormal();
                if (groundNormal.y < 0.98f)
                {
                    // Calculate direction to move along the slope
                    Vector3 upHill = -Vector3.Cross(Vector3.Cross(Vector3.up, groundNormal), groundNormal).normalized;

                    // Apply forward movement along the slope
                    Vector3 desiredVelocity = upHill * Speed;

                    // Keep only y and z if you're moving only in z direction
                    vel.y = desiredVelocity.y; // adds vertical movement
                    vel.z = Mathf.Lerp(vel.z, desiredVelocity.z, 0.2f);
                }
                else // flat 
                {
                    vel.z = Mathf.Lerp(vel.z, Speed, 0.2f);
                }
            }
            else
            {
                vel.z = Mathf.MoveTowards(vel.z, Speed, 1 * Time.fixedDeltaTime);
            }
            rb.linearVelocity = vel;
        }
    }

    private Vector3 GetGroundNormal()
    {
        Ray ray = new Ray(transform.position + transform.up * 0.2f, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.6f, LayerMask.GetMask("Road")))
        {
            return hitInfo.normal;
        }
        return Vector3.up;
    }

    // float beforeFrameX;
    // private void RaycastControl()
    // {
    //     Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); // Default forward rotation
    //     if (IsDown)
    //     {
    //         Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //         if (ControlRaycastPlane.Raycast(ray, out float enter))
    //         {
    //             //Get the point that is clicked
    //             beforeFrameX = ray.GetPoint(enter).x;

    //         }
    //     }

    //     if (IsClick)
    //     {
    //         Vector3 TargetPos = Vector3.zero;
    //         Ray ray = cam.ScreenPointToRay(Input.mousePosition);

    //         if (ControlRaycastPlane.Raycast(ray, out float enter))
    //         {
    //             //Get the point that is clicked
    //             Vector3 hitPoint = ray.GetPoint(enter);
    //             // Vector3 LocalConvert  = transform.InverseTransformPoint(hitPoint);
    //             float xDif = hitPoint.x - beforeFrameX;

    //             //Move your cube GameObject to the point where you clicked
    //             TargetPos = new Vector3(transform.localPosition.x + xDif, transform.localPosition.y, transform.localPosition.z); ;
    //             TargetPos.x = Mathf.Clamp(TargetPos.x, -5, 5);
    //             beforeFrameX = hitPoint.x;
    //         }
    //         // transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPos, 5 * Time.fixedDeltaTime);
    //         transform.localPosition = Vector3.Lerp(transform.localPosition, TargetPos, 0.5f);
    //         // transform.localPosition = TargetPos;

    //         // Check for significant movement to determine the rotation
    //         if (Mathf.Abs(transform.localPosition.x - TargetPos.x) > 0.1f)
    //         {
    //             float directionSign = Mathf.Sign(TargetPos.x - transform.localPosition.x);
    //             Vector3 moveDirection = new Vector3(directionSign, 0f, 1f).normalized;
    //             targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); // Rotate towards movement direction
    //         }
    //     }

    //     // Apply the target rotation smoothly in all cases
    //     childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
    // }

    // private void OldController()
    // {
    //     float horizontalInput = 0;
    //     // Move the player left and right
    //     if (IsClick)
    //     {
    //         horizontalInput = Input.GetAxisRaw("Mouse X");
    //     }
    //     // print(horizontalInput);
    //     float newPositionX = transform.localPosition.x + (horizontalInput * sideSpeed * Time.deltaTime);
    //     newPositionX = Mathf.Clamp(newPositionX, minXLimit, maxXLimit);
    //     Vector3 targetPos = new Vector3(newPositionX, transform.localPosition.y, transform.localPosition.z);
    //     transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.35f);
    //     if (Mathf.Abs(horizontalInput) > 0)
    //     {
    //         float rot = Mathf.Sign(horizontalInput);
    //         // print(rot);
    //         Vector3 moveDirection = new Vector3(rot, 0f, 1).normalized;
    //         // print(moveDirection);
    //         Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
    //         childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
    //     }
    //     else
    //     {
    //         childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, Quaternion.Euler(Vector3.forward), Time.deltaTime * rotSpeed);
    //     }
    // }

    // private void ViewPortControl()
    // {
    //     Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); // Default forward rotation

    //     if (IsClick)
    //     {
    //         // Convert mouse position to viewport position
    //         Vector3 viewPortPos = cam.ScreenToViewportPoint(Input.mousePosition);
    //         float inverse = Mathf.InverseLerp(0.1f, 0.9f, viewPortPos.x);
    //         // Calculate new X position within the bounds (-5 to 5)
    //         float newPositionX = Mathf.Lerp(minXLimit, maxXLimit, inverse);
    //         Vector3 targetPos = new Vector3(newPositionX, transform.localPosition.y, transform.localPosition.z);

    //         // Smoothly move the player to the target position
    //         transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.125f);

    //         // Check for significant movement to determine the rotation
    //         if (Mathf.Abs(transform.localPosition.x - newPositionX) > 0.5f)
    //         {
    //             float directionSign = Mathf.Sign(newPositionX - transform.localPosition.x);
    //             Vector3 moveDirection = new Vector3(directionSign, 0f, 1f).normalized;
    //             targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); // Rotate towards movement direction
    //         }

    //     }
    //     // Apply the target rotation smoothly in all cases
    //     childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
    // }
    Vector3 befFrameMous;
    float targetX;
    bool JustClicked = false;
    public ShouldDoMovement shouldDoMovement = ShouldDoMovement.None;
    private void ViewPortControl2()
    {
        SwipeAndPinch.TrackPos();
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up); // Default forward rotation
        if (IsDown || IsUp)
        {
            JustClicked = true;
            // SwipeAndPinch.ResetHighestY();
        }

        else
        if (IsClick)
        {
            if (JustClicked)
            {
                ResetTargetX();
                JustClicked = false;
            }
            Vector3 vel = rb.linearVelocity;
            vel.x = 0;
            rb.linearVelocity = vel;
            // Convert mouse position to viewport position
            Vector3 viewPortPos = cam.ScreenToViewportPoint(Pointer.current.position.ReadValue());
            float xDif = (viewPortPos.x - befFrameMous.x) * 40;
            targetX += xDif;
            Vector3 targetPos = new Vector3(targetX, transform.localPosition.y, transform.localPosition.z);
            // targetPos.x = Mathf.Clamp(targetPos.x, minXLimit, maxXLimit);
            befFrameMous = viewPortPos;

            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, 0.125f);
            // Smoothly move the player to the target position
            // Check for significant movement to determine the rotation
            if (Mathf.Abs(transform.localPosition.x - targetPos.x) > 0.5f)
            {
                float directionSign = Mathf.Sign(targetPos.x - transform.localPosition.x);
                Vector3 moveDirection = new Vector3(directionSign, 0f, 1f).normalized;
                targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up); // Rotate towards movement direction
            }
        }
        // if (IsUp && GetUIObjectUnderPointer() == null)
        if (IsUp || SwipeAndPinch.UpDrag())
        {
            // Jump();
            shouldDoMovement = ShouldDoMovement.Jump;
        }
        // if (SwipeAndPinch.GetSwipe() == SwipeAndPinch.SwipeDirection.Down)
        // {
        //     Slide();
        // }
        if (IsClick && SwipeAndPinch.DownDrag())
        {
            if (!isGrounded)
            {
                animController.SetRoll(true);
                rb.AddForce(Vector3.down * 500);
            }
            shouldDoMovement = ShouldDoMovement.Slide;
        }
        if (shouldDoMovement != ShouldDoMovement.None)
        {
            if (shouldDoMovement == ShouldDoMovement.Jump && CanJump())
            {
                Jump();
            }
            else if (shouldDoMovement == ShouldDoMovement.Slide && CanSlide())
            {
                Slide();
            }
            shouldDoMovement = ShouldDoMovement.None;
        }
        // Apply the target rotation smoothly in all cases
        childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
    }

    private bool CanSlide()
    {
        return Player.GetState() == PlayerState.Obs && isGrounded && slideCoroutine == null;
    }

    private bool CanJump()
    {
        return Player.GetState() == PlayerState.Obs && isGrounded && lastJumpFrame + jumpFrameSkipper <= Time.frameCount;
    }

    public void ResetTargetX()
    {
        // befFrameMous = cam.ScreenToViewportPoint(Input.mousePosition);
        befFrameMous = cam.ScreenToViewportPoint(Pointer.current.position.ReadValue());
        targetX = transform.localPosition.x;
        JustClicked = true;
    }

    public override void UseParentedMovement(bool val)
    {
        if (ParentedMove != val)
        {
            if (val)
            {
                Vector3 tileStart = Z.LS.LastInstLvl.PlayerBeingTile.start.position;
                Vector3 toBeParentPos = new Vector3(tileStart.x, tileStart.y, transform.position.z);
                // Vector3 toBeParentPos = transform.position;
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
    int jumpFrameSkipper = 1;
    int lastJumpFrame = 0;

    public void Jump()
    {
        // if (Player.GetState() == PlayerState.Obs && isGrounded && lastJumpFrame + jumpFrameSkipper <= Time.frameCount)
        // {
        transform.position += Vector3.up * 0.26f;
        lastJumpFrame = Time.frameCount;
        StopSlide();
        Vector3 vel = rb.linearVelocity;
        vel += Vector3.up * 9;
        // vel.z = 8;
        rb.linearVelocity = vel;
        // if (rb.linearVelocity.z < Speed / 1.3f)
        // {
        //     rb.linearVelocity += Vector3.forward * 3;
        // }
        print("Jumped");
        shouldDoMovement = ShouldDoMovement.None;
        // }
        // if (!isGrounded)
        // {
        //     print("Not Grounded");
        // }
    }
    [SerializeField] AnimationCurve jumpCurve;



    public void StopSlide()
    {
        if (slideCoroutine != null)
        {
            StopCoroutine(slideCoroutine);
            animController.Slide(false);
            animController.SetRoll(false);
            CapsuleCollider coliider = GetComponent<CapsuleCollider>();
            Vector3 center = coliider.center;
            coliider.height = 2f;
            center.y = 1f;
            coliider.center = center;
            slideCoroutine = null;
        }

    }

    public AnimationCurve slideCurve;
    Coroutine slideCoroutine = null;
    public void Slide()
    {
        // if (Player.GetState() == PlayerState.Obs)
        // {
        //     if (slideCoroutine == null)
        //     {
        //     }
        // }
        slideCoroutine = StartCoroutine(LocalCor());
        IEnumerator LocalCor()
        {
            float t = 0;
            float time = 0;
            float duration = 0.8f;
            animController.Slide(true);
            shouldDoMovement = ShouldDoMovement.None;
            CapsuleCollider coliider = GetComponent<CapsuleCollider>();
            Vector3 center = coliider.center;
            bool colliderAdjusted = false;
            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
                if (!colliderAdjusted && t >= 0.1f)
                {
                    coliider.height = 1f;

                    center.y = 0.5f;
                    coliider.center = center;
                    colliderAdjusted = true;
                }

                // Step 1: Get the character's forward direction
                Vector3 flatForward = (Vector3.forward - Vector3.up * 0.2f).normalized;
                // Step 2: Project it onto the slope
                Vector3 groundNormal = GetGroundNormal();
                Vector3 slopeForward = Vector3.ProjectOnPlane(flatForward, groundNormal).normalized;
                // Step 3: Use that as your desired direction
                Vector3 desiredVelocity = slopeForward * Speed * slideCurve.Evaluate(t);
                rb.linearVelocity = desiredVelocity;
                yield return null;
            }

            animController.Slide(false);
            animController.SetRoll(false);
            coliider.height = 2f;
            center.y = 1f;
            coliider.center = center;
            slideCoroutine = null;
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.25f, LayerMask.GetMask("Road"));
    }


}
public enum ShouldDoMovement
{
    None,
    Jump,
    Slide
}