using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SonidoAndaluz : MonoBehaviour
{
    private bool played = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!played && collision.CompareTag("Player"))
        {
            played = true;
            RuntimeManager.PlayOneShotAttached("event:/Secreto", this.gameObject);
        }
    }
}
