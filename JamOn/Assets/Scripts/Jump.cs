using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidbody;
    [SerializeField] private GroundDetector groundDetector;

    [Space]
    [SerializeField] private float jumpSpeed = 10.0f;

    [SerializeField] private float maxFallSpeed = 30.0f;
    [SerializeField] private float fallMultiplier = 4.5f;
    [SerializeField] private float lowJumpMultiplier = 4f;

    [SerializeField] private Animator playerAnim;

    private float jumpTimer = 0;
    private float groundedRemember = 0;
    private float inputTimer = 0.0f;

    [Range(0.1f, 1.0f)] [SerializeField] private float coyoteTime;
    [Tooltip("Tiempo que se queda guardado un inout de salto para que tenga efecto mas tarde")]
    [Range(0.0f, 1.0f)] [SerializeField] private float inputRemerberTime;

    private void Update()
    {
        // Coyote time
        if (jumpTimer > 0) jumpTimer -= Time.deltaTime;
        if (groundedRemember > 0) groundedRemember -= Time.deltaTime;
        if (inputTimer > 0.0) inputTimer -= Time.deltaTime;

        if (groundDetector.IsGrounded() && jumpTimer <= 0)
            groundedRemember = coyoteTime;

        if (!GameManager.Instance.GetInputFreeze() && Input.GetKeyDown(KeyCode.Space)) 
            inputTimer = inputRemerberTime;

        if(inputTimer > 0.0f)
            ExecuteJump();

        if(groundDetector.IsGrounded())
            playerAnim.SetBool("Falling", false);

        if (myRigidbody.velocity.y < 0)
        {
            if (Mathf.Abs(myRigidbody.velocity.y) > 2.5f)
                playerAnim.SetBool("Falling", true);

            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * myRigidbody.gravityScale * (fallMultiplier - 1.0f) * Time.deltaTime;

            if (Mathf.Abs(myRigidbody.velocity.y) > maxFallSpeed)
                myRigidbody.velocity = Vector2.up * -maxFallSpeed;
        }
        else if (myRigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1.0f) * Time.deltaTime;
        }
        else if (myRigidbody.velocity.y > 0 && Mathf.Abs(myRigidbody.velocity.y) < jumpSpeed * 0.75)
        {
            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1.0f) * Time.deltaTime;
        }
    }

    public void SetGravityScaleForTTime(float scale, float time)
    {
        StartCoroutine(InternalSetGravityScaleForTTime(scale, time));
    }

    private IEnumerator InternalSetGravityScaleForTTime(float scale, float time)
    {
        myRigidbody.gravityScale = scale;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        myRigidbody.gravityScale = 1.0f;
    }

    public void ExecuteJump()
    {
        if ((groundedRemember > 0 || groundDetector.IsGrounded()))
        {
            playerAnim.Play("PlayerJump");
            playerAnim.SetTrigger("Jump");
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
            myRigidbody.velocity += Vector2.up * jumpSpeed;

            groundedRemember = 0.0f;
        }
    }
}
