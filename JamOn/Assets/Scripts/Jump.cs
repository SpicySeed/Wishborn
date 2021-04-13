using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private GroundDetector groundDetector;

    [SerializeField] private float jumpHeight = 0.0f;
    [SerializeField] private float jumpCancelThreshold = 0.5f;

    [Range(0.0f, 1.0f)] [SerializeField] private float cancelRatio = 0.2f;

    [Space]
    [SerializeField] private AnimationCurve ascendCurve;
    [SerializeField] private AnimationCurve fallCurve;

    private float ascendTimer;
    private float fallTimer;

    private bool canJump = false;

    private float jumpTimer = 0;
    private float groundedRemember = 0;

    [Range(0.1f, 1.0f)] [SerializeField] private float coyoteTime;

    private void Update()
    {
        // Coyote time
        if (jumpTimer > 0) jumpTimer -= Time.deltaTime;
        if (groundedRemember > 0) groundedRemember -= Time.deltaTime;

        if (groundDetector.IsGrounded() && jumpTimer <= 0)
        {
            groundedRemember = coyoteTime;
        }

        if (groundDetector.IsGrounded()) canJump = true;

        CheckGravityChange();

        if (Input.GetKeyDown(KeyCode.Space))
            ExecuteJump();
        if (Input.GetKeyUp(KeyCode.Space))
            CancelJump();
    }

    public void ExecuteJump()
    {
        if ((groundedRemember > 0 || groundDetector.IsGrounded()) && canJump)
        {
            float vel = Mathf.Sqrt(2.0f * -Physics2D.gravity.y * myRigidbody.gravityScale * jumpHeight);
            myRigidbody.velocity *= Vector2.right;
            myRigidbody.AddForce(Vector2.up * vel, ForceMode2D.Impulse);

            canJump = false;

            ascendTimer = 0.0f;
            fallTimer = 0.0f;
            groundedRemember = 0.0f;
        }
    }

    public void CancelJump()
    {
        if (canJump || groundDetector.IsGrounded()) return;

        if(myRigidbody.velocity.y > jumpCancelThreshold)
            myRigidbody.velocity *= new Vector2(1.0f, 1.0f - cancelRatio);
    }

    public void CheckGravityChange()
    {
        if (myRigidbody.velocity.y > 0.0)
            ascendTimer += Time.deltaTime;

        if (myRigidbody.velocity.y <= 0.0)
            fallTimer += Time.deltaTime;

        if (!canJump && myRigidbody.velocity.y > 0.0)
            myRigidbody.gravityScale = ascendCurve.Evaluate(ascendTimer);
        else if (!canJump && myRigidbody.velocity.y <= 0.0)
            myRigidbody.gravityScale = fallCurve.Evaluate(fallTimer);
        else
            myRigidbody.gravityScale = 1.0f;
    }
}
