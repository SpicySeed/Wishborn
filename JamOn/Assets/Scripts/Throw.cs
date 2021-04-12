using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField]
    float offset = 1.0f;

    public void ThrowObject(GameObject objectToThrowPrefab, Vector3 throwForce)
    {
        GameObject gO = Instantiate(objectToThrowPrefab, transform.position + (throwForce.normalized * offset), Quaternion.identity);
        Rigidbody2D rb = gO.GetComponent<Rigidbody2D>();
        if (rb != null) rb.AddForce(throwForce, ForceMode2D.Impulse);
    }
}
