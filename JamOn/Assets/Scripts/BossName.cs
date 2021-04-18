using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossName : MonoBehaviour
{
    [SerializeField] private Key key;
    [SerializeField] private Animator anim;
    [SerializeField] private float delayTime = 1;
    [SerializeField] private float stayTime = 1;
    private bool start = false;
    private bool end = false;
    private float time = 0;

    void Update()
    {
        if (key.HasBeenPicked() && !start && time < delayTime)
        {
            time += Time.deltaTime;
            if (time > delayTime)
            {
                start = true;
                time = 0;
                anim.Play("NameAppear");
            }
        }

        if (start && !end && time < stayTime)
        {
            time += Time.deltaTime;
            if (time > stayTime)
            {
                end = true;
                anim.Play("NameDisappear");
            }
        }
    }
}
