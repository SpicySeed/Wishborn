using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private MovingFloor[] movingFloors;
    private Health playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        if (playerHealth.HasBeenRevived())
            Reset();
    }

    private void Reset()
    {
        foreach (MovingFloor mf in movingFloors)
            mf.Reset();
    }
}
