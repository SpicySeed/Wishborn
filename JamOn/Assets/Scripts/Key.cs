using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : Collectable
{
    [SerializeField] GameObject boss;
    [SerializeField] CinemachineShake shake;
    [SerializeField] SpriteRenderer tutorial;
    [SerializeField] SpriteRenderer keyRenderer;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;
    int picked = 0;


    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && picked == 0)
        {
            picked = 1;
            tutorial.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && picked == 1)
        {
            picked = 0;
            tutorial.enabled = false;
        }
    }

    private void Update()
    {
        if(picked == 1 && Input.GetKeyDown(KeyCode.E))
        {
            shake.ShakeCamera(shakeIntensity, shakeTime);
            keyRenderer.enabled = false;
            GameManager.Instance.SetInputFreeze(true);
            tutorial.enabled = false;
            picked = 2;
        }
        if (picked == 2 && !shake.isShaking())
        {
            GameManager.Instance.SetInputFreeze(false);
            picked = 3;
            boss.SetActive(true);
        }

    }
}
