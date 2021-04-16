using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Health : MonoBehaviour
{
    private Throwable throwable;
    private bool alive = true;

    private Vector2 respanwPosition;

    public Transform forceTranform;

    public float timeToRespawn = 1.0f;
    private float timer = 0.0f;

    [SerializeField] private GameObject mainBody;
    [SerializeField] private GameObject[] bodyParts;
    List<GameObject> instantiated;

    public ParticleSystem deathParticles;

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

        if (timer < 0.0f && !IsAlive()) Revive();
    }

    public void Die()
    {
        GameManager.Instance.SetInputFreeze(true);
        timer = timeToRespawn;

        deathParticles.Play();

        InstantiateDeathBody();
        mainBody.SetActive(false);

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

        deathParticles.Stop();
        GameManager.Instance.SetInputFreeze(false);
        mainBody.SetActive(true);
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

    private void InstantiateDeathBody()
    {
        for(int i = 0; i < bodyParts.Length; i++)
        {
            GameObject gO = Instantiate(bodyParts[i], bodyParts[i].transform.position, bodyParts[i].transform.rotation);
            gO.transform.localScale = mainBody.transform.localScale;
            SpriteSkin ss = gO.GetComponent<SpriteSkin>();
            if (ss != null) Destroy(ss);

            Collider2D col = gO.GetComponent<Collider2D>();
            if (col != null) Destroy(col);

            CircleCollider2D pCol = gO.AddComponent<CircleCollider2D>();
            Rigidbody2D rb = gO.AddComponent<Rigidbody2D>();
            rb.AddForce((gO.transform.position - forceTranform.position).normalized * 5.0f, ForceMode2D.Impulse);
            instantiated.Add(gO);
        }
    }

    private IEnumerator InternalDie()
    {
        yield return new WaitForSeconds(timeToRespawn - GameManager.Instance.GetTransitionManager().GetTransitionTime());
        StartCoroutine(GameManager.Instance.GetTransitionManager().StartTransition(TransitionManager.Transitions.CURTAIN, TransitionManager.Mode.WHOLE));
    }
}
