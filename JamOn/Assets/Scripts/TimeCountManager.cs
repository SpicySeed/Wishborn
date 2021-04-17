using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountManager : MonoBehaviour
{
    [SerializeField] Text timerText;

    float time = 0.0f;
    bool stopped = false;

    void Start()
    {
        time = 0.0f;
        gameObject.SetActive((GameManager.Instance.IsTimeModeActive()));
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            time += Time.deltaTime;
            int minutes = (Mathf.FloorToInt(time / 60));
            int seconds = (Mathf.FloorToInt(time % 60));
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StopTimer()
    {
        stopped = true;
    }
}
