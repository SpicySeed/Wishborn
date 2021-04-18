using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
public class DialogueManager : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter soundEmitter;
    public Text dialogueText;
    public GameObject endText;
    public bool dialoguefinished = false;
    public bool typing = false;
    public bool skip = false;
    public bool showEndText = false;

    DialogueTrigger dialogueTrigger;
    Queue<string> sentences;

    void Start()
    {
        dialogueTrigger = GetComponentInParent<DialogueTrigger>();
        sentences = new Queue<string>();

        dialogueTrigger.gameObject.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguefinished = false;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
            sentences.Enqueue(sentence);

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        endText.SetActive(false);
        typing = true;
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            soundEmitter.Play();

            if (skip)
            {
                dialogueText.text = sentence;
                skip = false;
                typing = false;
                endText.SetActive(true && showEndText);
                yield break;
            }
            else if (letter == '.')
                yield return new WaitForSeconds(0.2f);
            else if (letter == ',')
                yield return new WaitForSeconds(0.1f);
            else
                yield return new WaitForSeconds(0.02f);
        }

        skip = false;
        typing = false;
        endText.SetActive(true && showEndText);
    }

    public void EndDialogue()
    {
        dialogueText.text = "";
        if (dialogueTrigger != null)
            dialogueTrigger.GetComponent<Animator>().SetBool("active", false);

        dialoguefinished = true;
        RuntimeManager.PlayOneShotAttached("event:/closeDialogue", this.gameObject);
        endText.SetActive(false);
    }

    public bool IsDialogueFinished()
    {
        return dialoguefinished;
    }
}
