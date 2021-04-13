using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private bool alive = true;

    public void Die()
    {
        alive = false;
    }

    public void Revive()
    {
        //Respawn
    }

    public bool IsAlive()
    {
        return alive;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damage"))
            Die();
    }
}
