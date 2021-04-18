using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Experimental.Rendering.Universal;

public class Vase : Collectable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private StudioEventEmitter emitter;
    [SerializeField] private SpriteRenderer[] brokenParts;
    [SerializeField] private float brokenTime = 2.0f;

    private Health playerHealth = null;
    private GroundDetector groundDetector = null;
    public Transform forceTransform;
    private int collected = 0;
    private float timer = 0;

    List<GameObject> instantiated;

    public ParticleSystem soulParticles;
    public Light2D myLight;
    float intensity;

    private void Start()
    {
        instantiated = new List<GameObject>();
        intensity = myLight.intensity;
    }

    private void Update()
    {
        if (collected == 1)
        {
            if (!playerHealth.IsAlive())
            {
                collected = 0;
                spriteRenderer.enabled = true;
                Reset();
                soulParticles.Play();
                myLight.gameObject.SetActive(true);
                myLight.intensity = intensity;
                GameManager.Instance.CollectableReset();
            }
            else if (groundDetector.IsGrounded())
            {
                collected = 2;
                timer = 0.0f;
            }
        }
        if (collected == 2)
        {
            timer += Time.deltaTime;

            myLight.intensity = intensity - timer / brokenTime * intensity;
            if (timer > brokenTime)
            {
                gameObject.SetActive(false);
                myLight.gameObject.SetActive(false);
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collected == 0)
        {
            if (spriteRenderer.enabled) emitter.Play();
            spriteRenderer.enabled = false;
            Shatter();
            soulParticles.Stop();
            GameManager.Instance.ObjectCollected();
            playerHealth = collision.gameObject.GetComponent<Health>();
            groundDetector = collision.gameObject.GetComponent<Jump>().groundDetector;
            collected = 1;
        }
    }

    private void Reset()
    {
        foreach (GameObject go in instantiated)
        {
            Destroy(go);
        }
    }

    public void Shatter()
    {
        for (int i = 0; i < brokenParts.Length; i++)
        {
            GameObject gO = Instantiate(brokenParts[i].gameObject, brokenParts[i].gameObject.transform.position, brokenParts[i].transform.rotation, transform);
            gO.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            gO.SetActive(true);

            Collider2D col = gO.GetComponent<Collider2D>();
            if (col != null) Destroy(col);

            gO.layer = LayerMask.NameToLayer("AvoidPlayer");
            CircleCollider2D colision = gO.AddComponent<CircleCollider2D>();
            Rigidbody2D rb = gO.AddComponent<Rigidbody2D>();
            rb.AddForce((gO.transform.position - forceTransform.position).normalized * 5.0f, ForceMode2D.Impulse);
            instantiated.Add(gO);
        }
    }
}
