using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float stoppingDistance = 0.5f;
    [SerializeField] private float maxMoveSpeed = 0.1f;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 respawnOffset = new Vector2(-2.0f, -2.0f);
    [SerializeField] private bool chaseLastPosition = false;
    [SerializeField] private bool useTime;
    [SerializeField] private float newPosTime = 1.0f;

    private Health playerHealth;
    private List<Vector2> targetPos;
    private float timer = 0.0f;

    private void Start()
    {
        playerHealth = player.GetComponent<Health>();
        targetPos = new List<Vector2>();
        targetPos.Add(player.transform.position);
        transform.position = playerHealth.GetRespawnPosition() + respawnOffset;
    }

    private void Update()
    {
        if (chaseLastPosition)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, maxMoveSpeed);
        else
        {
            if (Vector2.Distance(transform.position, targetPos[0]) <= stoppingDistance)
            {
                targetPos.RemoveAt(0);
                if (!useTime || targetPos.Count == 0) targetPos.Add(player.transform.position);
            }

            if ((useTime && timer >= newPosTime))
            {
                targetPos.Add(player.transform.position);
                timer = 0.0f;
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPos[0], maxMoveSpeed);
            timer += Time.deltaTime;
        }

        if (!playerHealth.IsAlive())
        {
            transform.position = playerHealth.GetRespawnPosition() + respawnOffset;
            targetPos.Clear();
            targetPos.Add(playerHealth.GetRespawnPosition());
        }


    }
}
