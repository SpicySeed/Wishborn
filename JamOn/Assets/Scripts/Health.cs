using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [SerializeField] private Movement movement;
    private Throwable target;

    private bool alive = true;

    public bool IsAlive()
    {
        return alive;
    }

    public void SetTarget(Throwable target)
    {
        this.target = target;
    }

    private void Die()
    {
        if (target != null)
        {
            movement.ClearForces();
            transform.position = target.transform.position;
            Destroy(target.gameObject);
        }
        else
        {
            alive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.CompareTag("Weapon") || collision.collider.gameObject.CompareTag("Weapon"))
            Die();
    }
}
