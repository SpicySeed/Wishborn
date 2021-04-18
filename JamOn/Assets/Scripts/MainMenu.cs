using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GameManager gm = GameManager.Instance;
        gm.SetTimedModeActive(false);
        gm.NewRun();
        gm.LoadScene("Cinematic1");
    }

    public void LoadTimedGame()
    {
        GameManager gm = GameManager.Instance;
        gm.SetTimedModeActive(true);
        gm.NewRun();
        gm.LoadScene("Cinematic1");
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
