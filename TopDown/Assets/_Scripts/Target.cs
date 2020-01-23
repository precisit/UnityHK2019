using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : Damageable
{

    public GameObject HitEffect;

    private GameObject Chase;
    private NavMeshAgent agent;

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
    }

    void Update()
    {
        if(!isDead) {
            agent.SetDestination(Chase.transform.position);
            animator.SetFloat("MoveSpeed", 1.0f);
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
