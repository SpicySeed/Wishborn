using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private bool stopped = false;
    private TimeManager tm;

    void Start()
    {
        tm = TimeManager.Instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (stopped)
            {
                panel.SetActive(false);
                stopped = false;
                tm.Resume();
            }
            else
            {
                panel.SetActive(true);
                stopped = true;
                tm.Pause();
            }
        }
    }

    public void Resume()
    {
        panel.SetActive(false);
        stopped = false;
        tm.Resume();
    }

    public void LoadMainMenu()
    {
        GameManager.Instance.LoadMainMenu();
    }
}