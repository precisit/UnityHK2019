using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Damageable damageable;
    public AudioClip[] PassiveSounds;
    public ParticleSystem AliveParticles;

    [Header("PlayerDetection")]
    public GameObject Sensor;
    public LayerMask DetectionLayer;

    [Header("Enemy Stats")]
    public float MoveSpeed;
    public float Damage;

    private GameObject Chase;
    protected NavMeshAgent agent;
    private AudioSource audioSource;
    private float audioTime;

    private bool isDead;

    private CapsuleCollider capsuleCollider;

    public Animator animator;

    protected virtual void OnEnable()
    {
        damageable.OnDeath += OnDeath;
    }

    protected virtual void OnDisable()
    {
        damageable.OnDeath -= OnDeath;
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = MoveSpeed;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected virtual void Start()
    {
        Player p = FindObjectOfType<Player>();
        Chase = p.gameObject;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioTime = 0;
    }

    protected void Update()
    {
        if(!isDead) {
            agent.SetDestination(Chase.transform.position);
            animator.SetFloat("MoveSpeed", 1.0f);
            audioTime += Time.deltaTime;

            if(audioTime > 10f) {
                audioTime = 0;
                if(Random.value < 0.5f) {
                    audioSource.pitch = Random.Range(0.8f, 1.2f);
                    audioSource.PlayOneShot(PassiveSounds[(int) Random.value*PassiveSounds.Length]);
                }
            }
        }
    }

    protected virtual void OnDeath()
    {
       isDead = true;
       agent.speed = 0.0f;
       capsuleCollider.enabled = false;
       AliveParticles.Stop();
    }
}
