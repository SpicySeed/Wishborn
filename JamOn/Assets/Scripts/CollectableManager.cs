using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    [SerializeField] private Text collectableText;
    [SerializeField] private Transform collectablesParent;
    private Animator anim;
    private int numCollected = 0, maxNumber;

    private void Start()
    {
        maxNumber = collectablesParent.childCount;
        anim = GetComponent<Animator>();
        UpdateText();
    }

    public void ObjectReset()
    {
        numCollected--;
        UpdateText();
    }

    public void ObjectCollected()
    {
        numCollected++;
        UpdateText();
        if (anim == null) anim = GetComponent<Animator>();
        anim.Play("CollectableCollected");
    }

    private void UpdateText()
    {
        if (collectableText != null)
            collectableText.text = numCollected.ToString() + "/" + maxNumber.ToString();
    }
}
