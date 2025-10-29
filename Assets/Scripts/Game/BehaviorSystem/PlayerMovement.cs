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
    Player Player;
    public LayerMask groundLayer; // Layer mask for ground objects
    public float minXLimit = -8f; // Minimum X position limit
    public float maxXLimit = 8f; // Maximum X position limit
    public float rotSpeed = 10f; // Maximum X position limit
    public bool ParentedMove = true;
    Transform childModel;
    public bool ControlAble;
    public ZControlType ControlType = ZControlType.None;
    public BaseMovementState movementState = null;
    Camera cam;
    Plane ControlRaycastPlane;
    public bool addingForwardForece = false;

    public RunningState runningState = new RunningState();
    public SlidingState slideState = new SlidingState();
    public JumpingState jumpingState = new JumpingState();
    public DeadState deadState = new DeadState();
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
        movementState = runningState;
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
    public void SetMovementState(BaseMovementState state)
    {
        if (movementState != state)
        {
            movementState?.EndState(this);
            movementState = state;
            movementState?.BeginState(this);
        }
    }
    private void Update()
    {
        isGrounded = IsGrounded();
        if (movementState == null)
        {
            SetMovementState(runningState);
        }
        movementState.UpdateState(this);
        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     UseParentedMovement(true);
        // }
        // else if (Input.GetKeyDown(KeyCode.E))
        // {
        //     UseParentedMovement(false);
        // }
    }

    // void FixedUpdate()
    // {
    //     PlayerControl();
    //     // }
    // }

    public void PlayerControl()
    {
        movementState?.FixedUpdateState(this);
        // Move the player forward
        // transform.Translate(Vector3.forward * Speed * Time.deltaTime);

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
    public void Forward()
    {
        //Todo Animation Change using Change Animation
        addingForwardForece = false;
        // if (JustJumped)
        // {
        //     // Debug.Log($"Just last jump is {lastJumpFrame} Time frame is {Time.frameCount} ");
        //     return;
        // }

        if (isGrounded)
        {
            Vector3 vel = rb.linearVelocity;
            Vector3 flatForward = (transform.forward - Vector3.up * 0.5f).normalized;
            Vector3 groundNormal = GetGroundNormal();
            Vector3 slopeForward = Vector3.ProjectOnPlane(flatForward, groundNormal).normalized;
            Vector3 desiredVelocirt = slopeForward * Speed;

            Debug.DrawLine(transform.position, transform.position + slopeForward * 5, Color.red, 0.1f);
            if (vel.magnitude < Speed)
            {
                // vel = Vector3.Lerp(vel, desiredVelocirt, 0.125f);
                vel = desiredVelocirt;
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
            // rb.AddForce(Vector3.down * 5, ForceMode.Acceleration);

        }
        // else if (!isGrounded && rb.linearVelocity.y < 0.5f && forceDownGravity)
        // {
        //     rb.AddForce(Vector3.down * 10);
        // }
    }

    public Vector3 GetGroundNormal()
    {
        // Ray ray = new Ray(transform.position + transform.up * 0.2f, -transform.up);
        Ray ray = new Ray(transform.position + transform.up * 0.2f + Vector3.forward * 0.2f, -transform.up);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.6f, LayerMask.GetMask("Road")))
        {
            return hitInfo.normal;
        }
        return Vector3.up;
    }

    Vector3 befFrameMous;
    float targetX;
    bool JustClicked = false;
    public ShouldDoMovement shouldDoMovement = ShouldDoMovement.None;
    float shouldDoTime = 0;
    bool shouldDoTimeRecent => Time.time - shouldDoTime < 0.2f;
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
        if (IsClick && SwipeAndPinch.UpDrag())
        {
            // Jump();
            shouldDoMovement = ShouldDoMovement.Jump;
            shouldDoTime = Time.time;
        }
        if (IsClick && SwipeAndPinch.DownDrag())
        {
            if (!isGrounded)
            {
                // animController.SetRoll(true);
                rb.AddForce(Vector3.down * 500);
            }
            shouldDoMovement = ShouldDoMovement.Slide;
            shouldDoTime = Time.time;
        }
        if (shouldDoMovement != ShouldDoMovement.None && shouldDoTimeRecent)
        {
            if (shouldDoMovement == ShouldDoMovement.Jump && CanJump())
            {
                Jump();
            }
            else if (shouldDoMovement == ShouldDoMovement.Slide && CanSlide())
            {
                Slide();
            }
            // shouldDoMovement = ShouldDoMovement.None;
        }
        // Apply the target rotation smoothly in all cases
        childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
    }

    private bool CanSlide()
    {
        return Player.GetState() == PlayerState.Obs && isGrounded && movementState != slideState;
    }

    private bool CanJump()
    {
        return Player.GetState() == PlayerState.Obs && isGrounded && JustJumped;
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
    // int jumpFrameSkipper = 2;
    public int lastJumpFrame = 0;


    public void Jump()
    {

        SetMovementState(jumpingState);
        shouldDoMovement = ShouldDoMovement.None;
    }
    public bool JustJumped => lastJumpFrame + 4 >= Time.frameCount;
    [SerializeField] AnimationCurve jumpCurve;



    // public void StopSlide()
    // {
    //     if (slideCoroutine != null)
    //     {
    //         StopCoroutine(slideCoroutine);
    //         // animController.Slide(false);
    //         // animController.SetRoll(false);
    //         animController.ChangeAnimation("Run");
    //         CapsuleCollider coliider = GetComponent<CapsuleCollider>();
    //         Vector3 center = coliider.center;
    //         coliider.height = 2f;
    //         center.y = 1f;
    //         coliider.center = center;
    //         slideCoroutine = null;
    //     }

    // }

    public AnimationCurve slideCurve;
    // Coroutine slideCoroutine = null;
    public void Slide()
    {
        SetMovementState(slideState);
        shouldDoMovement = ShouldDoMovement.None;
        // slideCoroutine = StartCoroutine(LocalCor());
        // IEnumerator LocalCor()
        // {
        //     float t = 0;
        //     float time = 0;
        //     float duration = 0.8f;
        //     // animController.Slide(true);
        //     CapsuleCollider coliider = GetComponent<CapsuleCollider>();
        //     Vector3 center = coliider.center;
        //     bool colliderAdjusted = false;
        //     while (time < duration)
        //     {
        //         time += Time.deltaTime;
        //         t = time / duration;
        //         if (!colliderAdjusted && t >= 0.1f)
        //         {
        //             coliider.height = 1f;

        //             center.y = 0.5f;
        //             coliider.center = center;
        //             colliderAdjusted = true;
        //         }

        //         // Step 1: Get the character's forward direction
        //         Vector3 flatForward = (Vector3.forward - Vector3.up * 0.2f).normalized;
        //         // Step 2: Project it onto the slope
        //         Vector3 groundNormal = GetGroundNormal();
        //         Vector3 slopeForward = Vector3.ProjectOnPlane(flatForward, groundNormal).normalized;
        //         // Step 3: Use that as your desired direction
        //         Vector3 desiredVelocity = slopeForward * Speed * slideCurve.Evaluate(t);
        //         rb.linearVelocity = desiredVelocity;
        //         yield return null;
        //     }
        //     // animController.ChangeAnimation("Run");
        //     // animController.Slide(false);
        //     // animController.SetRoll(false);
        //     coliider.height = 2f;
        //     center.y = 1f;
        //     coliider.center = center;
        //     slideCoroutine = null;
        //     SetMovementState(runningState);
        // }
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position, 0.15f, LayerMask.GetMask("Road"));
    }
    // [SerializeField] bool forceDownGravity = false;

    // internal void SetGravity(bool v)
    // {
    //     forceDownGravity = v;
    // }
}
public enum ShouldDoMovement
{
    None,
    Jump,
    Slide
}