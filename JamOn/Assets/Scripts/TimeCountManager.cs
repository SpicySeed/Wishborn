using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCountManager : MonoBehaviour
{
    [SerializeField] Text timerText;

    void Start()
    {
        gameObject.SetActive((GameManager.Instance.IsTimeModeActive()));
    }

    public void SetTime(float time)
    {
        int minutes = (Mathf.FloorToInt(time / 60));
        int seconds = (Mathf.FloorToInt(time % 60));
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
