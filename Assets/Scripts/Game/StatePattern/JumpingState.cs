using UnityEngine;

public class JumpingState : BaseMovementState
{
    public override void BeginState(PlayerMovement manager)
    {
        manager.transform.position += Vector3.up * 0.25f;
        manager.lastJumpFrame = Time.frameCount;
        // StopSlide();
        Vector3 vel = manager.rb.linearVelocity;
        vel.y = 10;
        manager.rb.linearVelocity = vel;
        // animController.ChangeAnimation("Jump");
        // manager.SetGravity(true);
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
        if (manager.rb.linearVelocity.y > 0.5f)
        {
            manager.animController.ChangeAnimation("Jump");
        }
        else
        {
            manager.animController.ChangeAnimation("Fall");
        }

        if (!manager.isGrounded && manager.rb.linearVelocity.y < 0.1f)
        {
            // manager.rb.AddForce(Vector3.down * 10);
        }
        if (!manager.JustJumped && manager.isGrounded)
        {
            manager.SetMovementState(null);
        }
        Debug.Log("Jumping");
    }
}
