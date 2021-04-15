using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private Health playerHealth;

    [SerializeField] private MovingFloor[] movingFloors;
    // Más elementos de la sala

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        if (!playerHealth.IsAlive()) Reset();
    }

    private void Reset()
    {
        foreach (MovingFloor mf in movingFloors) mf.Reset();
        // Más elementos de la sala
    }
}
