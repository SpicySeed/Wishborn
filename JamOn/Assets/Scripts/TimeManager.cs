using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 2f;
    private bool stopped = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    void Update()
    {
        if (!stopped)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
    }

    public void DoSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1.0f;
    }

    public void Resume()
    {
        ResetTimeScale();
        stopped = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        stopped = true;
    }
}
