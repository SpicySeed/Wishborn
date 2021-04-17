using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private bool stopped = false;
    private TimeManager tm;
    [SerializeField] private StudioEventEmitter soundEmitter;

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
                if (!soundEmitter.IsPlaying())
                    soundEmitter.Play();
                soundEmitter.EventInstance.setParameterByName("TimeON", 1);

                //if (!soundEmitter.IsPlaying())
                //soundEmitter.Play();
            }
            else
            {
                panel.SetActive(true);
                stopped = true;
                tm.Pause();
                // soundEmitter.Stop();
                if (!soundEmitter.IsPlaying())
                    soundEmitter.Play();
                soundEmitter.EventInstance.setParameterByName("TimeON", 0);
                //soundEmitter.SetParameter();
                // 
                
            }
        }
    }

    public void Resume()
    {
        panel.SetActive(false);
        stopped = false;
        tm.Resume();
        if (!soundEmitter.IsPlaying())
            soundEmitter.Play();
        soundEmitter.EventInstance.setParameterByName("TimeON", 1);
        // if (!soundEmitter.IsPlaying())
        
    }

    public void LoadMainMenu()
    {
        GameManager gm = GameManager.Instance;
        gm.LoadScene(0);
        gm.ResetCurrentLevel();
        tm.Resume();
    }
}
