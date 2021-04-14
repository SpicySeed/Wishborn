using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] private Collider2D myCollider;

    private bool calculated = false;
    private bool grounded = false;

    [SerializeField] private LayerMask groundMask;

    private void LateUpdate()
    {
        calculated = false;
    }

    public bool IsGrounded()
    {
        if (calculated) return grounded;

        calculated = true;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(myCollider.bounds.center, myCollider.bounds.size, 0.0f);

        for(int i = 0; i < colliders.Length; i++)
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
