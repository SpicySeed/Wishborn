using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Throw playerThrow;

    [SerializeField] private GameObject throwablePrefab1;

    [SerializeField] private GameObject throwablePrefab2;
    [SerializeField] private float maxHoldDown = 2.5f;
    [SerializeField] private float forceMultiplier = 10.0f;
    private float holdDownTimer = 1.0f;


    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            InternalThrow(throwablePrefab1);
            TimeManager.Instance.ResetTimeScale();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            InternalThrow(throwablePrefab2);
            TimeManager.Instance.ResetTimeScale();
        }
        else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            holdDownTimer = 1.0f;
            TimeManager.Instance.DoSlowmotion();
        }

        holdDownTimer += Time.deltaTime * 2;
    }

    private void InternalThrow(GameObject prefab)
    {
        holdDownTimer = Mathf.Clamp(holdDownTimer, 1.0f, maxHoldDown);
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction.z = 0;
        direction.Normalize();
        Vector3 force = direction * forceMultiplier * holdDownTimer;
        playerThrow.ThrowObject(prefab, force);
    }
}
