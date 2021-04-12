using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
	public float slowdownFactor = 0.05f;
	public float slowdownLength = 2f;
	public static TimeManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
			Instance = this;
			DontDestroyOnLoad(gameObject);
			return;
        }
		Destroy(this);
    }

    void Update()
	{
		Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
		Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
	}

	public void DoSlowmotion()
	{
		Time.timeScale = slowdownFactor;
		Time.fixedDeltaTime = Time.timeScale * .02f;
	}

	public void ResetTimeScale()
    {
		Time.timeScale = 1.0f;
    }
}
