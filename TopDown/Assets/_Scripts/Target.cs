using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : Damageable
{

    public GameObject HitEffect;
    public AudioClip[] PassiveSounds;
    
    private GameObject Chase;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private float audioTime;

    private bool isDead;

    private CapsuleCollider capsuleCollider;

    public Animator animator;

 void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        Player p = FindObjectOfType<Player>();
        Chase = p.gameObject;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        audioTime = 0;
    }

    void Update()
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

    public override void OnDamage() {
       // Destroy(gameObject);
       animator.SetTrigger("Dead");
       isDead=true;
       agent.speed = 0.0f;
       capsuleCollider.enabled =false;
        //Debug.Log("Object destroyed");
    }

    public override GameObject GetHitEffect() {
        return Instantiate(HitEffect);
    }
}
