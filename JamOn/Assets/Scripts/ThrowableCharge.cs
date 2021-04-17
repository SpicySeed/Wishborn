using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class ThrowableCharge : Collectable
{
    [SerializeField] private float coolDown = 2;
    [SerializeField] private Animator anim;

    private float count = 0;

    private bool collected = false;

    private void Update()
    {
        if (collected)
        {
            count += Time.deltaTime;
            if (count > coolDown)
            {
                collected = false;
                anim.Play("ChargeAppear");
                count = 0;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected && collision.gameObject.CompareTag("Player"))
        {
            collected = true;
            RuntimeManager.PlayOneShotAttached("event:/Carga recuperada", this.gameObject);
            anim.Play("ChargeDisappear");
            collision.gameObject.GetComponent<Throw>().ChargeUp();
            TimeManager.Instance.DoSlowMotion();
        }
    }
}
