using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using FMODUnity;

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
    [SerializeField] private StudioEventEmitter soundEmitter;

    [SerializeField] private Throwable throwablePrefab;
    private Throwable throwable = null;
    private bool thrown = false;
    private bool stopped = false;
    private bool playingSound = false;

    [SerializeField] private float forceMultiplier = 10.0f;

    [SerializeField] private float movementScaleOnTeleport = 0.2f;
    [SerializeField] private float gravityScaleOnTeleport = 0.2f;
    [SerializeField] private float timeToWaitOnTeleport = 0.5f;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private ParticleSystem startCastingParticles;
    [SerializeField] private ParticleSystem castingParticles;
    [SerializeField] private ParticleSystem throwParticles;
    [SerializeField] private ParticleSystem appearParticles;

    private void Update()
    {
        if (!playerHealth.IsAlive())
        {
            startCastingParticles.Stop();
            castingParticles.Stop();
            throwParticles.Stop();
            appearParticles.Stop();
            soundEmitter.Stop();
            playingSound = false;
        }

        if (GameManager.Instance.GetInputFreeze()) return;

        if (Input.GetMouseButtonUp(0) && throwable == null && !thrown && !stopped)
        {
            throwable = InternalThrow(throwablePrefab);
            playerHealth.SetThrowable(throwable);
            thrown = true;
            orb.gameObject.SetActive(false);
            aimTarget.SetActive(false);

            playerAnim.SetTrigger("Throw");
            playerAnim.SetBool("Casting", false);

            startCastingParticles.Play();
            castingParticles.Stop();
            soundEmitter.SetParameter("Lanzar", 1);

            if (playingSound)
            {
                playingSound = false;
            }
        }
        else if (Input.GetMouseButtonUp(0) && throwable != null && !stopped)
        {
            throwable.Teleport(gameObject);
            playerHair.Teleport();
            orb.Reset();
            orb.Teleport();

            playerAnim.SetTrigger("Appear");
            appearParticles.Play();

            movement.ClearForces();
            movement.SetMovementScaleForTTime(movementScaleOnTeleport, timeToWaitOnTeleport);
            jump.SetGravityScaleForTTime(gravityScaleOnTeleport, timeToWaitOnTeleport);
            Destroy(throwable.gameObject);

            RuntimeManager.PlayOneShotAttached("event:/Teleport", this.gameObject);

        }
        else if (Input.GetMouseButtonDown(1) && throwable != null && !stopped)
        {
            orb.Reset();
            Destroy(throwable.gameObject);
            throwable = null;
            RuntimeManager.PlayOneShotAttached("event:/recuperar orbe", this.gameObject);
            thrown = false;
        }
        else if (Input.GetMouseButton(0) && throwable == null && !thrown && !stopped)
        {
            orb.GetCloser(orbSpawn);
            aimTarget.SetActive(true);
            Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - orbSpawn.position);
            direction.z = 0;
            aimTarget.transform.position = orbSpawn.position + direction.normalized * aimOffset;
            aimTarget.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));

            playerAnim.SetBool("Casting", true);
            castingParticles.Play();
            soundEmitter.Event = "event:/lanzamiento";

            if (!playingSound)
            {
                soundEmitter.Stop();
            }

            soundEmitter.SetParameter("Lanzar", 0);
            soundEmitter.OverrideAttenuation = true;
            soundEmitter.OverrideMaxDistance = 500;

            if (!playingSound)
            {
                playingSound = true;
                soundEmitter.Play();
            }
        }
        else if (throwable == null && !thrown)
        {
            playerAnim.SetBool("Casting", false);

            if (playingSound)
            {
                playingSound = false;
                soundEmitter.Stop();
            }

            orb.Reset();
            aimTarget.SetActive(false);
            castingParticles.Stop();

        }

        if (throwable == null && thrown && groundDetector.IsGrounded())
        {
            thrown = false;
            orb.Reset();

            startCastingParticles.Stop();
            castingParticles.Stop();
            throwParticles.Stop();
            appearParticles.Stop();
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

    public void ChargeUp()
    {
        thrown = false;
    }
}
