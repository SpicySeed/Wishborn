using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public void Teleport(GameObject player)
    {
        player.transform.position = gameObject.transform.position;
    }
}
