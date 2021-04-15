using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float deltaTimeMultiplier = 1.2f;

    private bool teleportEnabled = true;

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
        if (teleportEnabled)
        {
            Collider2D collider = player.GetComponent<Collider2D>();

            Collider2D[] collisions = Physics2D.OverlapBoxAll(transform.position, collider.bounds.size, 0.0f);
            Collider2D collision = null;
            System.Array.ForEach(collisions, (Collider2D c) =>
            {
                if (collision == null) collision = c;

                if (collision.transform.position.y > c.transform.transform.position.y)
                    collision = c;
            });

            if (collision != null)
                transform.position -= Vector3.up * collider.bounds.size.y;
            player.transform.position = transform.position;
        }
        else
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null) playerHealth.Die();
        }
    }

    public void SetTeleportEnabled(bool enabled)
    {
        this.teleportEnabled = enabled;
    }
}
