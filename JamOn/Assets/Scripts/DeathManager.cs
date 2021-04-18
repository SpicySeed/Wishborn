using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private Text deathsText;
    private Animator anim;
    private int numDeaths = 0;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayerDeath(int numDeaths)
    {
        this.numDeaths = numDeaths;
        UpdateText();
        if (anim == null) anim = GetComponent<Animator>();
        anim.Play("PlayerDeath");
    }

    private void UpdateText()
    {
        if (deathsText != null)
            deathsText.text = numDeaths.ToString();
    }
}
