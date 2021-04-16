using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private Animator playerAnim;

    private bool calculated = false;
    private bool grounded = false;

    [SerializeField] private LayerMask groundMask;

    [SerializeField] private ParticleSystem landParticles;

    private void Update()
    {
        bool currGrounded = grounded;
        IsGrounded();

        if (grounded && !currGrounded)
            landParticles.Play();

        playerAnim.SetBool("Grounded", grounded);
    }

    private void LateUpdate()
    {
        calculated = false;
    }

    public bool IsGrounded()
    {
        if (calculated) return grounded;

        calculated = true;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0.0f);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (((1 << colliders[i].gameObject.layer) & groundMask) != 0)
            {
                return grounded = true;
            }
        }
        return grounded = false;
    }

    public void ForceCalculate()
    {
        calculated = false;
    }
}
