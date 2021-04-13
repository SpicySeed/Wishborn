using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Throw playerThrow;
    [SerializeField] private Health playerHealth;

    [SerializeField] private Throwable teleporterPrefab;
    [SerializeField] private Throwable weaponPrefab;

    private Throwable teleporter = null;
    private Throwable weapon = null;

    [SerializeField] private float maxHoldDown = 2.5f;
    [SerializeField] private float forceMultiplier = 10.0f;
    private float holdDownTimer = 1.0f;


    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && teleporter == null)
        {
            teleporter = InternalThrow(teleporterPrefab);
            playerHealth.SetTarget(teleporter);
            TimeManager.Instance.ResetTimeScale();
        }
        else if (Input.GetMouseButtonUp(1) && weapon == null)
        {
            weapon = InternalThrow(weaponPrefab);
            TimeManager.Instance.ResetTimeScale();
        }
        else if ((Input.GetMouseButtonDown(0) && teleporter == null) || (Input.GetMouseButtonDown(1) && weapon == null))
        {
            holdDownTimer = 1.0f;
            TimeManager.Instance.DoSlowMotion();
        }

        holdDownTimer += Time.deltaTime * 2;
    }

    private Throwable InternalThrow(Throwable prefab)
    {
        holdDownTimer = Mathf.Clamp(holdDownTimer, 1.0f, maxHoldDown);
        Vector3 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        direction.z = 0;
        direction.Normalize();
        Vector3 force = direction * forceMultiplier * holdDownTimer;
        return playerThrow.ThrowObject(prefab, force);
    }
}
