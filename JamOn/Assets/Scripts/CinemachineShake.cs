using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    private float shakeTimer;
    private float shakeTimerTotal;
    private float startIntensity;

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float timer)
    {
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = timer;
        shakeTimerTotal = timer;
        startIntensity = intensity;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }

    public bool isShaking()
    {
        return shakeTimer > 0;
    }
}
