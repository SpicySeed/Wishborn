using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] private GroundDetector groundDetector;
    [SerializeField] private Movement movement;
    [SerializeField] private Jump jump;
    [SerializeField] private Hair playerHair;
    [SerializeField] private Health playerHealth;
    [SerializeField] private FollowingOrb orb;
    [SerializeField] private Transform orbSpawn;
    [SerializeField] private GameObject aimTarget;
    [SerializeField] private float aimOffset = 1.0f;

    [SerializeField] private Throwable throwablePrefab;
    private Throwable throwable = null;
    private bool thrown = false;
    private bool stopped = false;

    [SerializeField] private float forceMultiplier = 10.0f;

    [SerializeField] private float movementScaleOnTeleport = 0.2f;
    [SerializeField] private float gravityScaleOnTeleport = 0.2f;
    [SerializeField] private float timeToWaitOnTeleport = 0.5f;

    bool frozenInput = false;

    private void Update()
    {
        if (frozenInput) return;

        if (Input.GetMouseButtonUp(0) && throwable == null && !thrown && !stopped)
        {
            throwable = InternalThrow(throwablePrefab);
            playerHealth.SetThrowable(throwable);
            thrown = true;
            orb.gameObject.SetActive(false);
            aimTarget.SetActive(false);
        }
        else if (Input.GetMouseButtonUp(0) && throwable != null && !stopped)
        {
            throwable.Teleport(gameObject);
            playerHair.Teleport();
            orb.Reset();
            orb.Teleport();

            movement.ClearForces();
            movement.SetMovementScaleForTTime(movementScaleOnTeleport, timeToWaitOnTeleport);
            jump.SetGravityScaleForTTime(gravityScaleOnTeleport, timeToWaitOnTeleport);
            Destroy(throwable.gameObject);
        }
        else if (Input.GetMouseButtonDown(1) && throwable != null && !stopped)
        {
            orb.Reset();
            Destroy(throwable.gameObject);
            throwable = null;
            thrown = false;
        }
        else if (Input.GetMouseButton(0) && throwable == null && !stopped)
        {
            orb.GetCloser(orbSpawn);
            aimTarget.SetActive(true);
            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - orbSpawn.position);
            direction.z = 0;
            aimTarget.transform.position = orbSpawn.position + direction.normalized * aimOffset;
        }

        if (throwable == null && thrown && groundDetector.IsGrounded())
        {
            thrown = false;
            orb.Reset();
        }

        stopped = Time.timeScale == 0;
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

    public void FreezeInput(bool freeze)
    {
        frozenInput = freeze;
    }

    public void ChargeUp()
    {
        thrown = false;
    }
}
