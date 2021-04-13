using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Destroy(this.gameObject);
    }
}
