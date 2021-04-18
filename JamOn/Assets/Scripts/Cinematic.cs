using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour
{
    public GameObject Dialogue;
    [SerializeField] private float[] timePerDialogue;
    [SerializeField] private TransitionManager.Transitions[] dialogueTransitions;
    [SerializeField] private float defaultTimePerDialogue = 2.0f;
    [SerializeField] private Button skipButton;
    [SerializeField] private string nextSceneName;
    [SerializeField] private TransitionManager transitionManager;
    [SerializeField] private float initialDelay = 0.5f;

    int dialogueIndex = 0;

    DialogueTrigger dialogueTrigger;
    DialogueManager dialogueManager;

    bool end = false;
    bool canEnd = false;
    bool lineEnded = false;
    bool transitioned = false;

    float timer;

    public Dialogue dialogue;

    private void Start()
    {
        Invoke("InitCinematic", initialDelay);
    }

    private void InitCinematic()
    {
        dialogueManager = Dialogue.GetComponentInChildren<DialogueManager>();
        dialogueTrigger = Dialogue.GetComponent<DialogueTrigger>();
        if (timePerDialogue.Length < dialogue.sentences.Length)
        {
            timePerDialogue = new float[dialogue.sentences.Length];
            for (int i = 0; i < timePerDialogue.Length; i++)
                timePerDialogue[i] = defaultTimePerDialogue;
        }

        if (dialogueTransitions.Length < dialogue.sentences.Length)
        {
            dialogueTransitions = new TransitionManager.Transitions[dialogue.sentences.Length];
            for (int i = 0; i < dialogueTransitions.Length; i++)
                dialogueTransitions[i] = TransitionManager.Transitions.NONE;
        }

        StartCoroutine(StartTalking());
        canEnd = true;
        dialogueManager.dialoguefinished = false;
    }

    void Update()
    {
        if (!lineEnded && canEnd && !dialogueManager.typing)
        {
            timer = 0.0f;
            lineEnded = true;
        }

        if (lineEnded) timer += Time.deltaTime;

        if (canEnd && !dialogueManager.typing && timer >= (timePerDialogue[dialogueIndex] / 2.0f) && !transitioned)
        {
            transitioned = true;
            StartCoroutine(transitionManager.StartTransition(dialogueTransitions[dialogueIndex], TransitionManager.Mode.WHOLE));
        }

        if (canEnd && !dialogueManager.typing && timer >= timePerDialogue[dialogueIndex])
        {
            dialogueIndex++;
            dialogueManager.DisplayNextSentence();
            lineEnded = false;
            transitioned = false;
            timer = 0.0f;
        }

        if (canEnd && !end && dialogueManager.IsDialogueFinished())
        {
            end = true;
            canEnd = false;

            GameManager.Instance.SetInputFreeze(false);
            skipButton.gameObject.SetActive(false);
            EndCinematic();
        }

        if (Input.anyKeyDown && !skipButton.gameObject.activeSelf)
            skipButton.gameObject.SetActive(true);
    }

    IEnumerator StartTalking()
    {
        Dialogue.SetActive(true);
        dialogueTrigger.dialogue = dialogue;
        GameManager.Instance.SetInputFreeze(true);

        yield return 0.1f;
        dialogueTrigger.TriggerDialogue();
    }

    public void EndCinematic()
    {
        GameManager.Instance.LoadScene(nextSceneName);
    }
}
