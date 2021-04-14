using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    [SerializeField] private List<Transform> destinationPoints;
    [SerializeField] private Transform platformTransform;
    [SerializeField] private float speed;

    [SerializeField] private Vector2 center;
    [SerializeField] private Vector2 size;

    private Transform playerTransform;
    private Transform ballTransform;

    private int last = 0, next = 1;
    private float middlePos = 0;
    private bool calculated = false;

    private Collider2D[] currColliders = null;
    private Collider2D[] oldColliders = null;

    private void LateUpdate()
    {
        calculated = false;
        playerTransform = null;
        ballTransform = null;
    }

    void FixedUpdate()
    {
        CheckObjects();
        CheckInertia();

        Vector2 actualpos = platformTransform.position;
        Vector2 position = Vector2.Lerp(destinationPoints[last].position, destinationPoints[next].position, middlePos);
        float distance = Vector3.Distance(destinationPoints[last].position, destinationPoints[next].position);

        middlePos += (Time.fixedDeltaTime / distance) * speed;
        if (middlePos > 1)
        {
            middlePos = 0;
            last = (last + 1) % destinationPoints.Count;
            next = (next + 1) % destinationPoints.Count;
        }

        Vector2 positionDif = position - actualpos;

        if (playerTransform != null)
            playerTransform.position = new Vector3(playerTransform.position.x + positionDif.x, playerTransform.position.y + positionDif.y, playerTransform.position.z);

        if (ballTransform != null)
            ballTransform.position = new Vector3(ballTransform.position.x + positionDif.x, ballTransform.position.y + positionDif.y, ballTransform.position.z);

        platformTransform.position = position;
    }

    public void Reset()
    {
        last = 0;
        next = 1;
        middlePos = 0;
        platformTransform.position = destinationPoints[0].position;
    }

    private void CheckInertia()
    {
        if (oldColliders == null) return;

        for (int i = 0; i < oldColliders.Length; i++)
        {
            if (!System.Array.Exists(currColliders, (Collider2D c) => { return c == oldColliders[i]; }))
            {
                if (oldColliders[i] == null) continue;

                if (oldColliders[i].gameObject.CompareTag("Player"))
                {
                    Vector2 direction = destinationPoints[next].position - destinationPoints[last].position;
                    direction.Normalize();

                    // Horizontal inertia
                    Movement movement = oldColliders[i].gameObject.GetComponent<Movement>();
                    movement.LerpSpeed(speed * direction.x, 0.0f, 0.75f);

                    // Vertical inertia
                    if (direction.y > 0.0)
                    {
                        Rigidbody2D rB = oldColliders[i].gameObject.GetComponent<Rigidbody2D>();
                        rB.velocity += Vector2.up * direction.y * speed;
                    }

                    playerTransform = null;
                }

                if (oldColliders[i].gameObject.layer == LayerMask.NameToLayer("Orb"))
                    ballTransform = null;
            }
        }
    }

    private void CheckObjects()
    {
        if (calculated) return;

        calculated = true;
        oldColliders = currColliders;
        currColliders = Physics2D.OverlapBoxAll((Vector2)platformTransform.position + center, size * platformTransform.lossyScale * size, 0.0f);
   

        for (int i = 0; i < currColliders.Length; i++)
        {
            if (currColliders[i].gameObject.CompareTag("Player"))
            {
                playerTransform = currColliders[i].transform;
            }
            else if (currColliders[i].gameObject.layer == LayerMask.NameToLayer("Orb"))
            {
                ballTransform = currColliders[i].transform;
            }
        }
    }
    public void ForceCalculate()
    {
        calculated = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube((Vector2)platformTransform.position + center, size * platformTransform.lossyScale * size);
    }
}
