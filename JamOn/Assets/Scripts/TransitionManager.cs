using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public enum Transitions { FADE, CURTAIN, DIAMOND };
    public enum Mode { START, END, WHOLE };
    [SerializeField] private Animator[] transitions;
    [SerializeField] private float transitionTime = 1.0f;
    [SerializeField] private Transitions initialTransition = 0;

    private void Awake()
    {
        StartCoroutine(StartTransition(initialTransition, Mode.END));
    }

    public IEnumerator StartTransition(Transitions transitionType, Mode mode)
    {
        if ((int)transitionType >= transitions.Length) yield return null;

        transitions[(int)transitionType].gameObject.SetActive(true);

        if (mode == Mode.START || mode == Mode.WHOLE)
        {
            transitions[(int)transitionType].SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
        }
        if (mode == Mode.END || mode == Mode.WHOLE)
        {
            transitions[(int)transitionType].SetTrigger("End");
            yield return new WaitForSeconds(transitionTime);
        }

        transitions[(int)transitionType].gameObject.SetActive(false);
    }

    public IEnumerator StartTransitionAndLoad(Transitions transitionType, int sceneIndex)
    {
        if ((int)transitionType >= transitions.Length) yield return null;

        transitions[(int)transitionType].gameObject.SetActive(true);
        transitions[(int)transitionType].SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        GameManager.Instance.EndLoad();
        SceneManager.LoadScene(sceneIndex);
    }

    public float GetTransitionTime()
    {
        return transitionTime;
    }
}
