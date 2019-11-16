using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : Damageable
{

    public GameObject HitEffect;
    
    private GameObject Chase;
    private NavMeshAgent agent;

    void Start()
    {
        Player p = FindObjectOfType<Player>();
        Chase = p.gameObject;
        agent = GetComponent<NavMeshAgent>();
        
    }

    void Update()
    {
        agent.SetDestination(Chase.transform.position);
    }

    public override void OnDamage() {
        Destroy(gameObject);
        //Debug.Log("Object destroyed");
    }

    public override GameObject GetHitEffect() {
        return Instantiate(HitEffect);
    }
}
