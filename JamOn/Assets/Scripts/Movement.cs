using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D myRigidBody;

    [SerializeField] private float maxSpeed = 1.0f;

    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;

    private float currentAcceleration = 0.0f; // [0.0, 1.0]
    private int currentDirection = 0;

    private void Update()
    {
        float xAxis = Input.GetAxisRaw("Horizontal");
        Move(Mathf.RoundToInt(xAxis));

        if (currentDirection != 0)
            currentAcceleration += acceleration * currentDirection * Time.deltaTime;
        else if(currentAcceleration != 0.0)
        {
            float sign = Mathf.Sign(currentAcceleration);
            currentAcceleration -= deceleration * sign * Time.deltaTime;
            if (currentAcceleration * sign < 0.0)
                currentAcceleration = 0.0f;
        }

        currentAcceleration = Mathf.Clamp(currentAcceleration, -1.0f, 1.0f);
    }

    private void FixedUpdate()
    {
        myRigidBody.velocity = new Vector2(maxSpeed * currentAcceleration, myRigidBody.velocity.y);
    }

    public void ClearForces()
    {
        myRigidBody.velocity = Vector2.zero;
        myRigidBody.angularVelocity = 0;
    }

    public void Move(int direction) {
        if (direction < 0) direction = -1;
        if (direction > 0) direction = 1;

        currentDirection = direction;
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
