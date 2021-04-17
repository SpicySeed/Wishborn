using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathManager : MonoBehaviour
{
    [SerializeField] private Text deathsText;
    private int numDeaths = 0;

    public void PlayerDeath(int numDeaths)
    {
        this.numDeaths = numDeaths;
        UpdateText();
    }

    private void UpdateText()
    {
        if (deathsText != null)
            deathsText.text = numDeaths.ToString();
    }
}
