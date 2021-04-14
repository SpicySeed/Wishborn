using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private MovingFloor[] movingFloors;
    // Más elementos de la sala

    private void Update()
    {
        if (!playerHealth.IsAlive())
            Reset();
    }

    private void Reset()
    {
        playerHealth.Revive();
        foreach (MovingFloor mf in movingFloors) mf.Reset();
        // Más elementos de la sala
    }
}
