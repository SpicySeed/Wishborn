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
    [SerializeField] private StudioEventEmitter TimeEvent;
    private Animator playerAnim;

    void Start()
    {
        tm = TimeManager.Instance;
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (stopped)
            {
                panel.SetActive(false);
                stopped = false;
               
                if (!soundEmitter.IsPlaying())
                    soundEmitter.Play();
                soundEmitter.EventInstance.setParameterByName("TimeON", 1);
                tm.Resume();
                TimeEvent.Stop();
            }
            else
            {
                playerAnim.SetTrigger("Reset");
                panel.SetActive(true);
                stopped = true;
                if (!soundEmitter.IsPlaying())
                    soundEmitter.Play();
                soundEmitter.EventInstance.setParameterByName("TimeON", 0);
                tm.Pause();
                TimeEvent.Play();
            }
        }
    }

    public void Resume()
    {
        panel.SetActive(false);
        stopped = false;
        if(!soundEmitter.IsPlaying())
            soundEmitter.Play();
        soundEmitter.EventInstance.setParameterByName("TimeON", 1);
        tm.Resume();
        TimeEvent.Stop();
    }

    public void LoadMainMenu()
    {
        GameManager gm = GameManager.Instance;
        gm.LoadScene(0);
        gm.ResetCurrentLevel();
        tm.Resume();
        TimeEvent.Stop();
    }
}
