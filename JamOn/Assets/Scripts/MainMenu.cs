using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GameManager.Instance.SetTimedModeActive(false);
        GameManager.Instance.LoadScene(1);
    }

    public void LoadTimedGame()
    {
        GameManager.Instance.SetTimedModeActive(true);
        GameManager.Instance.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
