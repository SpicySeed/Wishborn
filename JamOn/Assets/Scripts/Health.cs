using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using FMODUnity;

public class Health : MonoBehaviour
{
    private Throwable throwable;
    private bool alive = true;
    private bool revived = true;

    private Vector2 respanwPosition;
    private Vector2 bossOffset = new Vector2(10.0f, 7.5f);

    public Transform forceTransform;

    public float timeToRespawn = 1.0f;
    private float timer = 0.0f;

    [SerializeField] private GameObject mainBody;
    [SerializeField] private SpriteRenderer[] bodyParts;
    List<GameObject> instantiated;

    public ParticleSystem deathParticles;
    public Animator playerAnim;


    private void Awake()
    {
        instantiated = new List<GameObject>();
    }

    private void Start()
    {
        respanwPosition = transform.position;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        revived = false;
        if (timer < 0.2f && !IsAlive()) deathParticles.Stop();
        if (timer < 0.0f && !IsAlive()) Revive();
    }

    public void Die()
    {
        GameManager.Instance.SetInputFreeze(true);
        GameManager.Instance.PlayerDeath();
        gameObject.layer = LayerMask.NameToLayer("PlayerDead");

        timer = timeToRespawn;
        RuntimeManager.PlayOneShotAttached("event:/Posible muerte", this.gameObject);
        deathParticles.Play();

        InstantiateDeathBody();

        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].enabled = false;
        }

        StartCoroutine(InternalDie());

        if (throwable != null) Destroy(throwable.gameObject);
        alive = false;
    }

    public void Revive()
    {
        foreach (GameObject go in instantiated)
        {
            Destroy(go);
        }

        GameManager.Instance.SetInputFreeze(false);
        gameObject.layer = LayerMask.NameToLayer("Player");

        playerAnim.SetTrigger("Reset");
        deathParticles.Stop();

        for (int i = 0; i < bodyParts.Length; i++)
        {
            bodyParts[i].enabled = true;
        }

        alive = true;
        transform.position = respanwPosition;
        revived = true;
    }

    public bool IsAlive()
    {
        return alive;
    }

    public bool HasBeenRevived()
    {
        return revived;
    }

    public void SetThrowable(Throwable throwable)
    {
        this.throwable = throwable;
    }

    public void SetRespawnPosition(Vector2 position, Vector2 offset)
    {
        respanwPosition = position;
        bossOffset = offset;
    }

    public Vector2 GetRespawnPosition()
    {
        return respanwPosition;
    }

    public Vector2 GetBossPos()
    {
        return bossOffset;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Damage") && alive)
            Die();
    }

    private void InstantiateDeathBody()
    {
        for (int i = 0; i < bodyParts.Length; i++)
        {
            GameObject gO = Instantiate(bodyParts[i].gameObject, bodyParts[i].gameObject.transform.position, bodyParts[i].transform.rotation);
            gO.transform.localScale = mainBody.transform.localScale;
            SpriteSkin ss = gO.GetComponent<SpriteSkin>();
            if (ss != null) Destroy(ss);

            Collider2D col = gO.GetComponent<Collider2D>();
            if (col != null) Destroy(col);

            CircleCollider2D pCol = gO.AddComponent<CircleCollider2D>();
            Rigidbody2D rb = gO.AddComponent<Rigidbody2D>();
            rb.AddForce((gO.transform.position - forceTransform.position).normalized * 5.0f, ForceMode2D.Impulse);
            instantiated.Add(gO);
        }
    }

    private IEnumerator InternalDie()
    {
        yield return new WaitForSeconds(timeToRespawn - GameManager.Instance.GetTransitionManager().GetTransitionTime());
        StartCoroutine(GameManager.Instance.GetTransitionManager().StartTransition(TransitionManager.Transitions.CURTAIN, TransitionManager.Mode.WHOLE));
    }
}
