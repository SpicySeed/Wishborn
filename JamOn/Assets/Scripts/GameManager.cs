using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int numLevels = 3;
    [SerializeField] private int levelOffset = 0;
    [SerializeField] Text collectableText; //TODO: mover esto al manager de la interfaz del nivel

    private int currentLevel = 1;
    private int[] collectablesCollected;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
            return;
        }
        Destroy(gameObject);
    }

    public void Init()
    {
        collectablesCollected = new int[numLevels];
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene(currentLevel);
    }

    public void ObjectCollected()
    {
        collectablesCollected[currentLevel]++;
        if (collectableText != null)
            collectableText.text = collectablesCollected[currentLevel + levelOffset].ToString();
    }

    public int GetObjectsCollected()
    {
        return collectablesCollected[currentLevel];
    }
}
