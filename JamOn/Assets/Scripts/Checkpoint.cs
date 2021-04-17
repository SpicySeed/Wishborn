using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] Transform bossOffset = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 offset = (bossOffset == null) ? Vector2.zero : (Vector2)bossOffset.position;
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null && playerHealth.IsAlive())
               playerHealth.SetRespawnPosition(transform.position, offset);
        }
    }
}
