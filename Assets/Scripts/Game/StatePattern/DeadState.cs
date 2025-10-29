using UnityEngine;

public class DeadState : BaseMovementState
{
    public override void BeginState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
        manager.animController.ChangeAnimation("Die");
    }

    public override void EndState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }

    public override void FixedUpdateState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }
}
