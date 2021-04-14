using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private TimeManager tm;
    private bool stopped = false;
    [SerializeField] private GameObject Panel;
    void Start()
    {
        tm = TimeManager.Instance;
    }

    public void Resume()
    {
        tm.Resume();
        Panel.SetActive(false);
        stopped = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (stopped)
            {
                tm.Resume();
                Panel.SetActive(false);
                stopped = false;
            }
            else
            {
                tm.Pause();
                Panel.SetActive(true);
                stopped = true;
            }
        }
    }
}
