using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private GroundDetector groundDetector;

    [Space]
    [SerializeField] private float jumpSpeed = 10.0f;
    [SerializeField] private float fallMultiplier = 4.5f;
    [SerializeField] private float lowJumpMultiplier = 4f;

    private float jumpTimer = 0;
    private float groundedRemember = 0;

    [Range(0.1f, 1.0f)] [SerializeField] private float coyoteTime;

    private void Update()
    {
        // Coyote time
        if (jumpTimer > 0) jumpTimer -= Time.deltaTime;
        if (groundedRemember > 0) groundedRemember -= Time.deltaTime;

        if (groundDetector.IsGrounded() && jumpTimer <= 0)
            groundedRemember = coyoteTime;


        if (Input.GetKeyDown(KeyCode.Space))
            ExecuteJump();
    
        if(myRigidbody.velocity.y < 0)
        {
            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1.0f) * Time.deltaTime;
        }
        else if(myRigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1.0f) * Time.deltaTime;
        }
    
    }

    public void ExecuteJump()
    {
        if ((groundedRemember > 0 || groundDetector.IsGrounded()))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
            myRigidbody.velocity += Vector2.up * jumpSpeed;

            groundedRemember = 0.0f;
        }
    }
}
