using UnityEngine;

public class JumpingState : BaseMovementState
{
    public override void BeginState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }

    public override void EndState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerMovement manager)
    {
        if (manager.rb.linearVelocity.y > 0.5f)
        {
            manager.animController.ChangeAnimation("Jump");
        }
        else
        {
            manager.animController.ChangeAnimation("Fall");
        }
        if (!manager.JustJumped && manager.isGrounded)
        {
            manager.SetMovementState(manager.runningState);
        }
    }
}
