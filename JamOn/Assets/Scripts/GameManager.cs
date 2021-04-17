using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int numLevels = 3;
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private CollectableManager collectableManager;

    private int currentLevel = 1;
    private bool loading = false;

    private bool inputFreeze = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Instance.transitionManager = this.transitionManager;
        Instance.collectableManager = this.collectableManager;
        Instance.loading = false;
        Destroy(gameObject);
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
        StartCoroutine(transitionManager.StartTransitionAndLoad(TransitionManager.Transitions.FADE, currentLevel));
    }

    public void ResetCurrentLevel()
    {
        currentLevel = 1;
    }

    public void ObjectCollected()
    {
        collectableManager.ObjectCollected();
    }

    public void CollectableReset()
    {
        collectableManager.ObjectReset();
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
