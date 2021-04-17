using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class DialogueTrigger : MonoBehaviour
{
    [HideInInspector]
    public Dialogue dialogue;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void TriggerDialogue()
    {
        anim.SetBool("active", true);
        RuntimeManager.PlayOneShotAttached("event:/openDialogue", this.gameObject);
        Invoke("StartDialogue", 0.5f);
    }

    void StartDialogue()
    {
        GetComponentInChildren<DialogueManager>().StartDialogue(dialogue);
    }
}
