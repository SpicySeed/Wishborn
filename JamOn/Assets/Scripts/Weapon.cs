using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Throwable
{
    public int bounceCount = 1;

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            bounceCount--;
            if (bounceCount < 0) Destroy(gameObject);
        }

        if (collision.otherCollider.gameObject.CompareTag("Player") || collision.collider.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
}
