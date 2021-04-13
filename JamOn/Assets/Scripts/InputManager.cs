using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GroundDetector groundDetector;

    [SerializeField] private Throwable throwablePrefab;
    private Throwable throwable = null;
    private bool thrown = false;

    [SerializeField] float offset = 1.0f;

    [SerializeField] private float maxHoldDown = 2.5f;
    [SerializeField] private float forceMultiplier = 10.0f;
    private float holdDownTimer = 1.0f;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && throwable == null && !thrown)
        {
            throwable = InternalThrow(throwablePrefab);
            thrown = true;
        }
        else if (Input.GetMouseButtonUp(0) && throwable != null)
        {
            throwable.Teleport(gameObject);
            Destroy(throwable.gameObject);

            holdDownTimer = 1.0f;
        }

        holdDownTimer += Time.deltaTime * 2;
    }

    private void FixedUpdate()
    {
        if (throwable == null && thrown && groundDetector.IsGrounded())
            thrown = false;
    }

    private Throwable ThrowObject(Throwable throwable, Vector3 throwForce)
    {
        Throwable aux = Instantiate(throwable, transform.position + (throwForce.normalized * offset), Quaternion.identity);
        Rigidbody2D rb = aux.GetComponent<Rigidbody2D>();
        if (rb != null) rb.AddForce(throwForce, ForceMode2D.Impulse);
        return aux;
    }

    private Throwable InternalThrow(Throwable prefab)
    {
        holdDownTimer = Mathf.Clamp(holdDownTimer, 1.0f, maxHoldDown);
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction.z = 0;
        direction.Normalize();
        Vector3 force = direction * forceMultiplier * holdDownTimer;
        return ThrowObject(prefab, force);
    }
}
