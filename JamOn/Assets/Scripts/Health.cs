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

    public void Die(Vector3 portDirection)
    {
        if (target != null)
        {
            movement.ClearForces();
            transform.position = target.transform.position; //+ (portDirection * 5.0f);
            movement.SetKnifeDirection(portDirection);
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
            alive = false; //Die();
    }
}
