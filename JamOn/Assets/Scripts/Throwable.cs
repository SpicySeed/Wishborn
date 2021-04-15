using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D myCollider;
    [SerializeField] private float deltaTimeMultiplier = 1.2f;

    public void Update()
    {
        float increasedDeltaTime = deltaTimeMultiplier * Time.deltaTime;
        rb.velocity += Physics2D.gravity / rb.mass * increasedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damage"))
            Destroy(gameObject);
    }

    public void Teleport(GameObject player)
    {
        Collider2D[] barriers = Physics2D.OverlapBoxAll(transform.position, myCollider.bounds.size, 0.0f, 1 << LayerMask.NameToLayer("Barriers"));
        bool teleportEnabled = barriers.Length == 0;
        if (!teleportEnabled)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null) playerHealth.Die();
        }
        else
        {
            Collider2D collider = player.GetComponent<Collider2D>();

            Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, 1.25f * collider.bounds.size, 0.0f);

            if (collisions.Length > 0)
                transform.position -= Vector3.up * collider.bounds.size.y * 0.8f; ;
            player.transform.position = transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, myCollider.bounds.size);
    }
}
