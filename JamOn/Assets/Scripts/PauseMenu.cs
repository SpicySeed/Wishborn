using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
                RuntimeManager.PlayOneShotAttached("event:/Reaundar", this.gameObject);
            }
            else
            {
                panel.SetActive(true);
                stopped = true;
                tm.Pause();
                RuntimeManager.PlayOneShotAttached("event:/Pausar", this.gameObject);
            }
        }
    }

    public void Resume()
    {
        panel.SetActive(false);
        stopped = false;
        tm.Resume();
        RuntimeManager.PlayOneShotAttached("event:/Reaundar", this.gameObject);
    }

    public void LoadMainMenu()
    {
        GameManager gm = GameManager.Instance;
        gm.LoadScene(0);
        gm.ResetCurrentLevel();
        tm.Resume();
    }
}
