using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int numLevels = 3;
    [SerializeField] Text collectableText; //TODO: mover esto al manager de la interfaz del nivel
    [SerializeField] private TransitionManager transitionManager;

    private int currentLevel = 1;
    private int[] collectablesCollected;
    private bool loading = false;

    private bool inputFreeze = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
            return;
        }
        Instance.transitionManager = this.transitionManager;
        Instance.loading = false;
        Destroy(gameObject);
    }

    public void Init()
    {
        collectablesCollected = new int[numLevels];
    }

    public void LoadScene(int index)
    {
        if (loading) return;

        loading = true;
        StartCoroutine(transitionManager.StartTransitionAndLoad(TransitionManager.Transitions.FADE, index));
    }

    public void LoadNextLevel()
    {
        if (loading) return;

        loading = true;
        currentLevel++;
        StartCoroutine(transitionManager.StartTransitionAndLoad(TransitionManager.Transitions.CURTAIN, currentLevel));
    }

    public void ResetCurrentLevel()
    {
        currentLevel = 1;
    }

    public void ObjectCollected()
    {
        collectablesCollected[currentLevel]++;
        if (collectableText != null)
            collectableText.text = collectablesCollected[currentLevel].ToString();
    }

    public int GetObjectsCollected()
    {
        return collectablesCollected[currentLevel];
    }

    public void CollectableReset()
    {
        collectablesCollected[currentLevel]--;
        if (collectableText != null)
            collectableText.text = collectablesCollected[currentLevel].ToString();
    }

    public TransitionManager GetTransitionManager()
    {
        return transitionManager;
    }

    public void SetInputFreeze(bool freeze)
    {
        inputFreeze = freeze;
    }

    public bool GetInputFreeze()
    {
        return inputFreeze;
    }
}
