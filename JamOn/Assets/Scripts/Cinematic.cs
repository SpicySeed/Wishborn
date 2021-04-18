using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : MonoBehaviour
{
    public GameObject Dialogue;
    [SerializeField] private float[] timePerDialogue;
    [SerializeField] private float defaultTimePerDialogue = 2.0f;

    int dialogueIndex = 0;

    DialogueTrigger dialogueTrigger;
    DialogueManager dialogueManager;

    bool end = false;
    bool canEnd = false;
    bool lineEnded = false;

    float timer;

    public Dialogue dialogue;

    private void Start()
    {
        dialogueManager = Dialogue.GetComponentInChildren<DialogueManager>();
        dialogueTrigger = Dialogue.GetComponent<DialogueTrigger>();
        if (timePerDialogue.Length < dialogue.sentences.Length)
        {
            timePerDialogue = new float[dialogue.sentences.Length];
            for (int i = 0; i < timePerDialogue.Length; i++)
                timePerDialogue[i] = defaultTimePerDialogue;
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

        if (canEnd && !dialogueManager.typing && timer >= timePerDialogue[dialogueIndex])
        {
            dialogueIndex++;
            dialogueManager.DisplayNextSentence();
            lineEnded = false;
        }

        if (canEnd && !end && dialogueManager.IsDialogueFinished())
        {
            end = true;
            canEnd = false;

            GameManager.Instance.SetInputFreeze(false);
        }
    }

    IEnumerator StartTalking()
    {
        Dialogue.SetActive(true);
        dialogueTrigger.dialogue = dialogue;
        GameManager.Instance.SetInputFreeze(true);

        yield return 0.1f;
        dialogueTrigger.TriggerDialogue();
    }
}
