using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    public Image imagePanel;
    public Button nextButton;

    public GameObject p1;
    public GameObject p2;

    public float fadeTime = 1.0f;

    public Text deathText;
    public Text timeText;
    public Text collectText;

    private void Awake()
    {
        imagePanel.gameObject.SetActive(true);
        p1.SetActive(false);
        p2.SetActive(false);

        NextPanel();

        deathText.text = GameManager.Instance.GetNumDeaths().ToString();
        float time = GameManager.Instance.GetRunTime();
        int minutes = (Mathf.FloorToInt(time / 60));
        int seconds = (Mathf.FloorToInt(time % 60));
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        collectText.text = GameManager.Instance.GetCollectablesCollected().ToString();
    }

    public void NextPanel()
    {
        StartCoroutine(InternalNext());
    }

    private IEnumerator InternalNext()
    {
        nextButton.gameObject.SetActive(false);
        imagePanel.gameObject.SetActive(true);
        float timer = fadeTime;

        // Fade in
        while (timer > 0.0f)
        {
            imagePanel.color = new Color(0.0f, 0.0f, 0.0f, 1.0f - timer / fadeTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        if (!p1.activeSelf && !p2.activeSelf)
        {
            p1.SetActive(true);
            p2.SetActive(false);
        }
        else if (p1.activeSelf)
        {
            p1.SetActive(false);
            p2.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(0);
            yield break;
        }

        timer = fadeTime;

        // Fade out
        while (timer > 0.0f)
        {
            imagePanel.color = new Color(0.0f, 0.0f, 0.0f, timer / fadeTime);
            timer -= Time.deltaTime;
            yield return null;
        }

        imagePanel.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);

    }
}
