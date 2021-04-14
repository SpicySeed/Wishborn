using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private Jump jump;
    [SerializeField] private Hair playerHair;
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private Transform orbSpawn;

    [SerializeField] private Throwable throwablePrefab;
    private Throwable throwable = null;
    private bool thrown = false;
    private bool stopped = false;
    [SerializeField] private float forceMultiplier = 10.0f;

    [SerializeField] private float movementScaleOnTeleport = 0.2f;
    [SerializeField] private float gravityScaleOnTeleport = 0.2f;
    [SerializeField] private float timeToWaitOnTeleport = 0.5f;

    private void Update()
    {
       
        
        if (Input.GetMouseButtonUp(0) && throwable == null && !thrown && !stopped)
        {
            throwable = InternalThrow(throwablePrefab);
            thrown = true;
        }
        else if (Input.GetMouseButtonUp(0) && throwable != null && !stopped)
        {
            Vector3 earlyPos = transform.position;
            throwable.Teleport(gameObject);
            Vector3 laterPos = transform.position;
            Vector3 newHairPos = laterPos - earlyPos;
            playerHair.Teleport(newHairPos);
            movement.ClearForces();
            movement.SetMovementScaleForTTime(movementScaleOnTeleport, timeToWaitOnTeleport);
            jump.SetGravityScaleForTTime(gravityScaleOnTeleport, timeToWaitOnTeleport);
            Destroy(throwable.gameObject);
        }
        else if (Input.GetMouseButtonDown(1) && throwable != null && !stopped)
        {
            Destroy(throwable.gameObject);
            throwable = null;
            thrown = false;
        }

        if (throwable == null && thrown && groundDetector.IsGrounded())
            thrown = false;

        if (Time.timeScale == 0)
            stopped = true;
        else
            stopped = false;
    }

    private Throwable ThrowObject(Throwable throwable, Vector3 throwForce)
    {
        Throwable aux = Instantiate(throwable, orbSpawn.transform.position, Quaternion.identity);
        Rigidbody2D rb = aux.GetComponent<Rigidbody2D>();
        if (rb != null) rb.AddForce(throwForce, ForceMode2D.Impulse);
        return aux;
    }

    private Throwable InternalThrow(Throwable prefab)
    {
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction.z = 0;
        direction.Normalize();
        Vector3 force = direction * forceMultiplier;
        return ThrowObject(prefab, force);
    }

    public void ChargeUp()
    {
        thrown = false;
    }
}
