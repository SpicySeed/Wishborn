using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talk : MonoBehaviour
{
    public GameObject Dialogue;

    DialogueTrigger dialogueTrigger;
    DialogueManager dialogueManager;

    GameObject player;

    bool dialogueStarted = false;
    bool end = false;

    bool canEnd = false;

    public GameObject tutorial;

    public Dialogue dialogue;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueTrigger = Dialogue.GetComponent<DialogueTrigger>();
        dialogueManager = Dialogue.GetComponentInChildren<DialogueManager>();
        tutorial.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (tutorial.activeSelf)
            {
                StartCoroutine(StartTalking());
                dialogueStarted = true;
                canEnd = true;
            }
            else if ( dialogueManager.gameObject.activeSelf && dialogueManager.endText.activeSelf)
                dialogueManager.DisplayNextSentence();
        }
        
        /*if (!Dialogue.activeInHierarchy && dialogueStarted)
        {
            dialogueStarted = false;

            /*if (playFX != "")
                AudioManager.instance.Play(playFX);

            GetComponent<BoxCollider2D>().enabled = false;
            enabled = false;
        }*/

        if (canEnd && !end && dialogueManager.IsDialogueFinished())
        {
            end = true;
            dialogueStarted = false;

            //dialogueManager.EndDialogue();
            //Dialogue.SetActive(false);

            GameManager.Instance.SetInputFreeze(false);
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            player = coll.gameObject;
            tutorial.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            player = null;
            tutorial.SetActive(false);
        }
    }

    IEnumerator StartTalking()
    {
        tutorial.SetActive(false);
        Dialogue.SetActive(true);

        print("starttalking");

        //dialogueTrigger.GetComponent<Animator>().SetBool("active", true);
        dialogueTrigger.dialogue = dialogue;

        if (player != null)
        {
            GameManager.Instance.SetInputFreeze(true);
        }

        yield return 0.1f;

        dialogueTrigger.TriggerDialogue();
    }
}