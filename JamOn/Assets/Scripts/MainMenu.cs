using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadGame()
    {
        GameManager.Instance.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
