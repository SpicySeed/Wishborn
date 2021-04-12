using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Collider2D myCollider;

    private bool grounded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            grounded = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            grounded = false;
    }

    public bool IsGrounded()
    {
        return grounded;
    }

    public void ForceGrounded(bool grounded)
    {
        this.grounded = grounded;
    }
}
