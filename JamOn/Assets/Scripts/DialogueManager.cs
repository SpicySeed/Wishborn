using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public GameObject endText;
    DialogueTrigger dialogueTrigger;
    bool dialoguefinished = false;

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
        //if (!endText.activeSelf) return;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        //print("display next");

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        endText.SetActive(false);

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;

            // TODO: AQUI HACER SONIDO DE ESCRIBIR LETRA
            //AudioManager.instance.Stop("Write");
            //AudioManager.instance.Play("Write");

            if (letter == '.') // || letter == ',' || letter == '?'
                yield return new WaitForSeconds(0.2f);
            else
                yield return new WaitForSeconds(0.02f); ;
        }

        endText.SetActive(true);
    }

    public void EndDialogue()
    {
        dialogueText.text = "";
        if(dialogueTrigger != null)
            dialogueTrigger.GetComponent<Animator>().SetBool("active", false);
        dialoguefinished = true;

        endText.SetActive(false);
    }

    public bool IsDialogueFinished()
    {
        return dialoguefinished;
    }
}