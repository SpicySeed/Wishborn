using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField]
    float offset = 1.0f;

    public Throwable ThrowObject(Throwable objectToThrowPrefab, Vector3 throwForce, Health thrower)
    {
        Throwable gO = Instantiate(objectToThrowPrefab, transform.position + (throwForce.normalized * offset), Quaternion.identity);
        gO.SetThrower(thrower);
        Rigidbody2D rb = gO.GetComponent<Rigidbody2D>();
        if (rb != null) rb.AddForce(throwForce, ForceMode2D.Impulse);
        return gO;
    }
}
