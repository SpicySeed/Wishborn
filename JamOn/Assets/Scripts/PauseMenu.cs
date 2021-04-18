using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject optionsPanel;
    private bool stopped = false;
    private TimeManager tm;
    [SerializeField] private StudioEventEmitter soundEmitter;
    [SerializeField] private StudioEventEmitter TimeEvent;
    private Throw playerThrow;

    void Start()
    {
        tm = TimeManager.Instance;
        playerThrow = GameObject.FindGameObjectWithTag("Player").GetComponent<Throw>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsOnDialogue()) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (stopped)
            {
                panel.SetActive(false);
                pausePanel.SetActive(true);
                optionsPanel.SetActive(false);
                stopped = false;
               
                if (!soundEmitter.IsPlaying())
                    soundEmitter.Play();
                soundEmitter.EventInstance.setParameterByName("TimeON", 1);
                tm.Resume();
                TimeEvent.Stop();
                playerThrow.StopCasting();
            }
            else
            {
                playerThrow.StopCasting();
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
