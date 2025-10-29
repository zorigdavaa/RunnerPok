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

    public override void UpdateState(PlayerMovement manager)
    {
        if (manager.isGrounded)
        {
            manager.animController.ChangeAnimation("Run");
        }
    }
}
