using UnityEngine;

public class SlidingState : BaseMovementState
{
    CapsuleCollider coliider;
    Vector3 center;
    bool colliderAdjusted;
    float duration = 0.8f;
    float time = 0;
    float t = 0;
    public override void BeginState(PlayerMovement manager)
    {
        // manager.animController.ChangeAnimation("LandRoll");
        coliider = manager.GetComponent<CapsuleCollider>();
        center = coliider.center;
        colliderAdjusted = false;
        time = 0;
        t = 0;
    }

    public override void EndState(PlayerMovement manager)
    {
        // animController.ChangeAnimation("Run");
        coliider.height = 2f;
        center.y = 1f;
        coliider.center = center;
        // slideCoroutine = null;
    }

    public override void FixedUpdateState(PlayerMovement manager)
    {
        // throw new System.NotImplementedException();
    }

    public override void UpdateState(PlayerMovement manager)
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
        Vector3 groundNormal = manager.GetGroundNormal();
        Vector3 slopeForward = Vector3.ProjectOnPlane(flatForward, groundNormal).normalized;
        // Step 3: Use that as your desired direction
        Vector3 desiredVelocity = slopeForward * manager.Speed * manager.slideCurve.Evaluate(t);
        manager.rb.linearVelocity = desiredVelocity;
        manager.animController.ChangeAnimation("LandRoll");
        if (t >= 1)
        {
            manager.SetMovementState(null);
        }
    }
}
