using UnityEngine;

public class RunningState : BaseMovementState
{
    public override void BeginState(PlayerMovement manager)
    {

    }

    public override void EndState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }

    public override void FixedUpdateState(PlayerMovement manager)
    {
        if (manager.ParentedMove)
        {
            manager.playerParent.Translate(Vector3.forward * manager.Speed * Time.deltaTime);
            Vector3 vel = manager.rb.linearVelocity;
            vel.z = 0;
            manager.rb.linearVelocity = vel;
        }
        else
        {
            // OldForward();
            manager.Forward();
        }
    }

    public override void UpdateState(PlayerMovement manager)
    {
        if (manager.isGrounded)
        {
            manager.animController.ChangeAnimation("Run");
        }
        else
        {
            if (manager.rb.linearVelocity.y < 0.5f)
            {
                manager.animController.ChangeAnimation("Fall");
            }
        }
    }
}
