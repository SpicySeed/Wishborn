using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    Health thrower;

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colGO = collision.collider.gameObject;
        GameObject otherColGO = collision.otherCollider.gameObject;
        if (otherColGO.layer == LayerMask.NameToLayer("Ground") || colGO.layer == LayerMask.NameToLayer("Ground"))
            Destroy(this.gameObject);

        if (otherColGO.CompareTag("Weapon") || colGO.CompareTag("Weapon"))
        {
            Rigidbody2D rb = otherColGO.GetComponent<Rigidbody2D>();

            if (rb == null)
                rb = colGO.GetComponent<Rigidbody2D>();

            if (rb == null)
                Debug.LogError("ERROR: Weapon no tiene RigidBody2D!");
            
            thrower.Die(rb.velocity);
            Destroy(gameObject);
        }
    }

    public void SetThrower(Health thrower)
    {
        this.thrower = thrower;
    }
}
