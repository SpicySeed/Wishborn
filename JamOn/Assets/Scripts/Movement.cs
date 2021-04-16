using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidBody;

    [SerializeField] private float maxSpeed = 1.0f;
    private float inertiaSpeed = 0.0f;

    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;
    [SerializeField] private StudioEventEmitter soundEmitter;

    private float movementScale = 1.0f;

    private float currentAcceleration = 0.0f; // [0.0, 1.0]
    private int currentDirection = 0;
    private bool sound = false;
    [SerializeField] private Transform targetDir;
    [SerializeField] private Transform orbDir;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private ParticleSystem runParticles;

    private ParticleSystem.EmissionModule emission;

    private void Start()
    {
        emission = runParticles.emission;
    }

    private void Update()
    {
        if (!GameManager.Instance.GetInputFreeze())
        {
            float xAxis = Input.GetAxisRaw("Horizontal");
            Move(Mathf.RoundToInt(xAxis));
        }
        else if (currentDirection != 0)
            currentDirection = 0;

        if (currentDirection != 0)
        {
            currentAcceleration += acceleration * currentDirection * Time.deltaTime;

            emission.enabled = true;
            playerAnim.SetBool("Running", true);
            if (!sound)
            {
                soundEmitter.Play();
                sound = true;
            }
            

            // Flip & hair rotation
            if (currentDirection == 1)
            {
                transform.localScale = new Vector3(1, 1, 1);
                targetDir.eulerAngles = new Vector3(0, 0, 135);
                orbDir.eulerAngles = new Vector3(0, 0, 180);

                runParticles.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (currentDirection == -1)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                targetDir.eulerAngles = new Vector3(0, 0, 45);
                orbDir.eulerAngles = new Vector3(0, 0, 0);

                runParticles.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else if (currentAcceleration != 0.0)
        {
            float sign = Mathf.Sign(currentAcceleration);
            currentAcceleration -= deceleration * sign * Time.deltaTime;
            if (currentAcceleration * sign < 0.0)
                currentAcceleration = 0.0f;
        }
        else
        {
            if (sound)
            {
                soundEmitter.Stop();
                sound = false;
            }
            playerAnim.SetBool("Running", false);
            emission.enabled = false;
        }

        currentAcceleration = Mathf.Clamp(currentAcceleration, -1.0f, 1.0f);
    }

    private void FixedUpdate()
    {
        myRigidBody.velocity = new Vector2(inertiaSpeed + maxSpeed * currentAcceleration, myRigidBody.velocity.y) * movementScale;
    }

    public void SetMovementScaleForTTime(float scale, float time)
    {
        StartCoroutine(InternalSetMovementScaleForTTime(scale, time));
    }

    private IEnumerator InternalSetMovementScaleForTTime(float scale, float time)
    {
        movementScale = scale;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        movementScale = 1.0f;
    }

    public void ClearForces()
    {
        myRigidBody.velocity = Vector2.zero;
        myRigidBody.angularVelocity = 0;
    }

    public void Move(int direction)
    {
        if (direction < 0) direction = -1;
        if (direction > 0) direction = 1;

        currentDirection = direction;
    }

    public void LerpSpeed(float from, float to, float time)
    {
        StartCoroutine(InternalLerpSpeed(from, to, time));
    }

    private IEnumerator InternalLerpSpeed(float from, float to, float time)
    {
        float ogTime = time;
        while (time > 0.0f)
        {
            inertiaSpeed = Mathf.Lerp(from, to, ogTime - time);
            time -= Time.deltaTime;
            yield return null;
        }
        inertiaSpeed = to;
    }

    public void SetMaxSpeed(float maxSpeed)
    {
        this.maxSpeed = maxSpeed;
    }

    public void SetAcceleration(float acceleration)
    {
        this.acceleration = acceleration;
    }

    public void SetDeceleration(float deceleration)
    {
        this.deceleration = deceleration;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetAcceleration()
    {
        return acceleration;
    }

    public float GetDeceleration()
    {
        return deceleration;
    }
}
