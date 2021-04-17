using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float maxMoveSpeed = 0.1f;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform chasePosition;
    [SerializeField] private Vector2 respawnOffset = new Vector2(-2.0f, -2.0f);
    [SerializeField] private bool chaseLastPosition = false;
    [SerializeField] private bool useTime;
    [SerializeField] private float newPosTime = 1.0f;
    [SerializeField] private Transform hairDir;
    [SerializeField] private Transform tailDir;
    [SerializeField] private Hair hair;
    [SerializeField] private Hair tail;
    [SerializeField] private Animator anim;

    private Health playerHealth;
    private List<Vector2> targetPos;
    private float timer = 0.0f;

    private void Start()
    {
        playerHealth = player.GetComponent<Health>();
        targetPos = new List<Vector2>();
        targetPos.Add(chasePosition.position);
        transform.position = playerHealth.GetRespawnPosition() + respawnOffset;
    }

    private void Update()
    {
        if (playerHealth.IsAlive())
        {
            if (chaseLastPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, chasePosition.position, maxMoveSpeed);
                FlipCharacter(chasePosition.position);
            }
            else
            {
                if (Vector2.Distance(transform.position, targetPos[0]) <= stoppingDistance)
                {
                    targetPos.RemoveAt(0);
                    if (!useTime || targetPos.Count == 0) targetPos.Add(chasePosition.position);
                }

                if ((useTime && timer >= newPosTime))
                {
                    targetPos.Add(chasePosition.position);
                    timer = 0.0f;
                }

                transform.position = Vector2.MoveTowards(transform.position, targetPos[0], maxMoveSpeed);
                FlipCharacter(targetPos[0]);
                timer += Time.deltaTime;
            }
        }

        if (playerHealth.HasBeenRevived())
        {
            transform.position = playerHealth.GetRespawnPosition() + respawnOffset;
            hair.Teleport();
            tail.Teleport();
            targetPos.Clear();
            targetPos.Add(chasePosition.position);
            FlipCharacter(targetPos[0]);
            anim.Play("Boss_Idle");
        }


    }

    private void FlipCharacter(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        if (dir.x >= 0.0f)
        {
            transform.localScale = Vector3.one;
            hairDir.eulerAngles = new Vector3(0, 0, 120);
            tailDir.eulerAngles = new Vector3(0, 0, -120);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            hairDir.eulerAngles = new Vector3(0, 0, 60);
            tailDir.eulerAngles = new Vector3(0, 0, -60);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.CompareTag("Player") || collision.collider.CompareTag("Player"))
            anim.Play("Boss_Laugh");
    }
}
