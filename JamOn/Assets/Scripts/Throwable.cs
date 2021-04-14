using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float deltaTimeMultiplier = 1.2f;

    private bool tpEnabled = true;

    public void Teleport(GameObject player)
    {
        if (tpEnabled)
            player.transform.position = transform.position;
        else
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null) playerHealth.Die();
        }
    }

    public void Update()
    {
        float increasedDeltaTime = deltaTimeMultiplier * Time.deltaTime;
        rb.velocity += Physics2D.gravity / rb.mass * increasedDeltaTime;
    }

    public void SetTeleportEnabled(bool enabled)
    {
        this.tpEnabled = enabled;
    }
}
