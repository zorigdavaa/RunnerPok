using UnityEngine;

public abstract class BaseMovementState
{
    public abstract void BeginState(PlayerMovement manager);
    public abstract void UpdateState(PlayerMovement manager);
    public abstract void FixedUpdateState(PlayerMovement manager);
    public abstract void EndState(PlayerMovement manager);
}
