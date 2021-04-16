using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Coin : Collectable
{    
    private int collected = 0;
    private Health playerHealth = null;
    private GroundDetector groundDetector = null;
    private SpriteRenderer spriteRenderer = null;
    [SerializeField] private StudioEventEmitter emitter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (collected == 1)
        {
            if (!playerHealth.IsAlive())
            {
                collected = 0;
                spriteRenderer.enabled = true;
                GameManager.Instance.CollectableReset();
            }
            else if (groundDetector.IsGrounded())
            {
                collected = 2;
                gameObject.SetActive(false);
            }
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collected == 0)
        {
            if (spriteRenderer.enabled)
                emitter.Play();
            spriteRenderer.enabled = false;
            GameManager.Instance.ObjectCollected();
            playerHealth = collision.gameObject.GetComponent<Health>();
            groundDetector = collision.gameObject.GetComponent<Jump>().groundDetector;
            collected = 1;
        }
    }
}
