using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class NextLevelDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            RuntimeManager.PlayOneShotAttached("event:/posible pasar de nivel", this.gameObject);
            if (playerHealth != null && playerHealth.IsAlive())
                GameManager.Instance.LoadNextLevel();
        }
    }
}
