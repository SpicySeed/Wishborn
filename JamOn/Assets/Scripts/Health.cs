using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private Throwable throwable;
    private bool alive = true;

    private Vector2 respanwPosition;

    private void Start()
    {
        respanwPosition = transform.position;
    }

    private void Update()
    {
        if (!IsAlive()) Revive();
    }

    public void Die()
    {
        StartCoroutine(GameManager.Instance.GetTransitionManager().StartTransition(TransitionManager.Transitions.CURTAIN, TransitionManager.Mode.WHOLE));
        if (throwable != null) Destroy(throwable.gameObject);
        alive = false;
    }

    public void Revive()
    {
        alive = true;
        transform.position = respanwPosition;
    }

    public bool IsAlive()
    {
        return alive;
    }

    public void SetThrowable(Throwable throwable)
    {
        this.throwable = throwable;
    }

    public void SetRespawnPosition(Vector2 position)
    {
        respanwPosition = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damage"))
            Die();
    }
}
