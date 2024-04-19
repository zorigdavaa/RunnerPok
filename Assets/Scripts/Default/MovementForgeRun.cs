using UnityEngine;
using System;
using ZPackage;

///<Summary>Forge Run Controller 1<Summary>
public class MovementForgeRun : Mb
{
    public float Speed = 5f; // Speed at which the player moves forward
    public BaseAnim animController;
    public float jumpForce = 8f; // Force applied when jumping
    [SerializeField] float MaxSpeed = 10;
    [SerializeField] Transform targetPos;
    [SerializeField] Vector3? NoLookTaget;
    private Rigidbody rb;
    public bool isGrounded;
    public Transform playerParent;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public bool ControlAble;
    // void FixedUpdate()
    // {
    //     if (targetPos)
    //     {
    //         ForwardMove(targetPos.position);
    //         // childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);
    //     }
    //     else if (NoLookTaget != null)
    //     {
    //         MoveNoLook();
    //         // childModel.localRotation = Quaternion.Lerp(childModel.localRotation, Quaternion.Euler(0, 0, 0), Time.fixedDeltaTime * 5);
    //     }

    // }
    // float CheckSign(float value)
    // {
    //     if (value <= -0.01f)
    //         return -1f;
    //     else if (value >= 0.01f)
    //         return 1f;
    //     else
    //         return 0f; // Or handle other cases as needed
    // }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    public void SetSpeed(float percent)
    {
        Speed = MaxSpeed * percent;
        // foreach (var node in player.Nodes)
        // {
        //     node.SetSpeed(speed / MaxSpeed);
        // }        
        //Todo Players Node move
        animController.SetSpeed(Speed / MaxSpeed);
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

    public virtual void UseParentedMovement(bool v)
    {

    }
}
