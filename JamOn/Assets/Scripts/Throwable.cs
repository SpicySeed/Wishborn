using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.tag == "Floor" ||collision.collider.tag == "Floor")
            Destroy(this.gameObject);
    }
}
