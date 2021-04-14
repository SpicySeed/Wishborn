using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOrbOnBarrier : MonoBehaviour
{
    [SerializeField] Throwable throwable;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Barriers"))
            throwable.SetTeleportEnabled(false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Barriers"))
            throwable.SetTeleportEnabled(true);
    }
}
