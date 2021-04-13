using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableCharge : Collectable
{
    [SerializeField] private float coolDown = 2;
    private float count = 0;

    private bool collected = false;

    private SpriteRenderer sprite = null;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (collected)
        {
            count += Time.deltaTime;
            if (count > coolDown)
            {
                collected = false;
                sprite.enabled = true;
                count = 0;
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collected && collision.gameObject.CompareTag("Player"))
        {
            collected = true;
            sprite.enabled = false;
            collision.gameObject.GetComponent<Throw>().ChargeUp();
        }
    }
}
