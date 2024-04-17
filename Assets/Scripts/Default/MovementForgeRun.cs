using UnityEngine;
using System;
using ZPackage;

///<Summary>Forge Run Controller 1<Summary>
public class MovementForgeRun : Mb
{
    public Player player;
    bool ControlAble;
    [SerializeField] float MaxSpeed = 10;
    [SerializeField] Transform targetPos;
    [SerializeField] Vector3? NoLookTaget;
    public float Speed = 5f; // Speed at which the player moves forward
    public float sideSpeed = 5f; // Speed at which the player moves left and right
    public float jumpForce = 8f; // Force applied when jumping
    public Transform groundCheck; // Transform representing a point at the bottom of the player to check for ground
    public LayerMask groundLayer; // Layer mask for ground objects
    public float minXLimit = -5f; // Minimum X position limit
    public float maxXLimit = 5f; // Maximum X position limit
    public float rotSpeed = 50f; // Maximum X position limit

    private Rigidbody rb;
    public bool isGrounded;
    Transform childModel;
    public Transform playerParent;
    private void Start()
    {
        groundLayer = LayerMask.GetMask("Road");
        childModel = transform.GetChild(0);
        playerParent = transform.parent;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (IsPlaying)
        {
            if (ControlAble)
            {
                // Check if the player is grounded
                isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer);
                if (isGrounded)
                {
                    player.animationController.Jump(false);
                }
                else
                {
                    player.animationController.Jump(true);
                    if (rb.velocity.y < 0f)
                    {
                        rb.velocity += Vector3.down * 0.8f;
                    }
                    // rb.velocity += Vector3.down * 1.2f;
                }

                // Move the player forward
                // transform.Translate(Vector3.forward * Speed * Time.deltaTime);
                playerParent.Translate(Vector3.forward * Speed * Time.deltaTime);
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
                    print(moveDirection);
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
                    childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, targetRotation, Time.deltaTime * rotSpeed);
                }
                else
                {
                    childModel.transform.rotation = Quaternion.Lerp(childModel.rotation, Quaternion.Euler(Vector3.forward), Time.deltaTime * rotSpeed);
                }
            }
            else if (targetPos)
            {
                ForwardMove(targetPos.position);
                // childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);
            }
            else if (NoLookTaget != null)
            {
                MoveNoLook();
                // childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);
            }
        }
    }
    float CheckSign(float value)
    {
        if (value <= -0.01f)
            return -1f;
        else if (value >= 0.01f)
            return 1f;
        else
            return 0f; // Or handle other cases as needed
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    internal void SetSpeedDirect(float value)
    {
        Speed = value;
    }

    public void SetSpeed(float percent)
    {
        Speed = MaxSpeed * percent;
        // foreach (var node in player.Nodes)
        // {
        //     node.SetSpeed(speed / MaxSpeed);
        // }        
        //Todo Players Node move
        player.animController.SetSpeed(Speed / MaxSpeed);
    }
    public float GetSpeed()
    {
        return Speed;
    }
    public void GoToPosition(Vector3 _target, bool noLook, float speedPercent = 1, Action afterAction = null)
    {
        SetSpeed(speedPercent);
        targetPos = null;
        NoLookTaget = _target;
        CancelDistance = 0.1f;
        afterGoAction = afterAction;
    }
    public void GoToPosition(Transform _target, float _cancelDistance = 0.1f, float speedPercent = 1, Action afterAction = null)
    {
        SetSpeed(speedPercent);
        targetPos = _target;
        CancelDistance = _cancelDistance;
        afterGoAction = afterAction;
    }
    public Action afterGoAction;
    public void GoToPosition(Vector3 _target, float _cancelDistance = 0.1f, Action afterAction = null)
    {
        SetSpeed(1);
        CancelDistance = _cancelDistance;
        GameObject Goto = new GameObject("Goto");
        Goto.transform.position = _target;
        targetPos = Goto.transform;
        afterGoAction = afterAction;
    }
    public void Cancel(bool cancelAfterInvoke = false)
    {
        if (targetPos && targetPos.name == "Goto")
        {
            Destroy(targetPos.gameObject);
        }
        targetPos = null;
        NoLookTaget = null;
        SetSpeed(0);
        if (cancelAfterInvoke)
        {
            afterGoAction = null;
        }
        afterGoAction?.Invoke();
    }
    public void SetControlAble(bool value)
    {
        ControlAble = value;
    }

    public void ClampPosition()
    {
        if (transform.position.x <= -6.8f || transform.position.x >= 6.8f)
        {
            var pos = rb.position;
            pos.x = Mathf.Clamp(rb.position.x, -6.8f, 6.8f);
            rb.position = pos;
        }
    }
    bool IsGrounded()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 1.05f);

        return hitInfo.collider != null;
    }
    bool IsOnAir()
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo, 2f);
        // print(hitInfo.collider == null);
        return hitInfo.collider == null;
    }

    Collider FrontRayCast(float length)
    {
        RaycastHit hitInfo;
        float rayCastHeight = 0.2f;
        for (int i = 0; i < 3; i++)
        {
            Physics.Raycast(transform.position + Vector3.up * rayCastHeight, transform.forward, out hitInfo, length);
            rayCastHeight += 0.7f;
            if (hitInfo.collider)
            {
                return hitInfo.collider;
            }
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        //IsGrounded ray
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 1.05f, Color.blue);
        //IsOnAir ray
        Debug.DrawRay(transform.position + Vector3.up, Vector3.down * 2.0f, Color.cyan);
        //FrontRay
        Debug.DrawRay(transform.position + Vector3.up * 1.2f, transform.forward * 0.31f, Color.cyan);
    }
    float CancelDistance = 0.1f;
    public void ForwardMove(Vector3 _targetPos)
    {
        _targetPos.y = 0;
        Vector3 lookTaget = _targetPos;
        lookTaget.y = 0;
        transform.LookAt(lookTaget);
        // Vector3 forwardMove = Vector3.forward * Speed * Time.fixedDeltaTime;
        Vector3 forwardMove = transform.forward * Speed * Time.fixedDeltaTime;
        // rb.MovePosition(transform.position + forwardMove);
        rb.position += forwardMove;
        float distance = Vector3.Distance(transform.position, _targetPos);
        if (distance < CancelDistance)
        {
            Cancel();
        }

        ClampPosition();
    }
    private void MoveNoLook()
    {
        rb.position = Vector3.MoveTowards(rb.position, NoLookTaget.Value, Speed * Time.fixedDeltaTime);
        float distance = Vector3.Distance(transform.position, NoLookTaget.Value);
        if (distance < CancelDistance)
        {
            Cancel();
        }

    }
}
