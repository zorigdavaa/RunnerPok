using UnityEngine;

public class SlidingState : BaseMovementState
{
    public override void BeginState(PlayerMovement manager)
    {
        manager.animController.ChangeAnimation("LandRoll");
    }

    public override void EndState(PlayerMovement manager)
    {
        // manager.SetMovementState(manager.runningState);
    }

    public override void UpdateState(PlayerMovement manager)
    {
        manager.animController.ChangeAnimation("LandRoll");
    }
}
