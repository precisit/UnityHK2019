using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy
{
    public float DamageDelay;
    public Transform DamageTransform;
    public float DamageRadius;
    public LayerMask DamageLayer;

    private enum State
    {
        Walk,
        Attack,
        Dead
    }
    private State currentState;

    protected override void Awake()
    {
        base.Awake();
        SetState(State.Walk);
    }

    private void SetState(State newState, float time = 0f)
    {
        if (time > 0f)
        {
            StartCoroutine(DelayState(newState, time));
        }
        else
        {
            currentState = newState;

            switch(newState)
            {
                case State.Walk:
                    agent.speed = MoveSpeed;
                    break;
                case State.Attack:
                    animator.SetTrigger("Attack");
                    agent.speed = 0.0f;
                    break;
            }
        }
    }

    private IEnumerator DelayState(State newState, float time)
    {
        yield return new WaitForSeconds(time);
        SetState(newState);
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        animator.SetTrigger("Dead");
        SetState(State.Dead);
    }

    protected virtual void OnTriggerEnter(Collider collider)
    {
        if (DetectionLayer == (DetectionLayer | (1 << collider.gameObject.layer)))
        {
            if (currentState == State.Walk)
            {
                SetState(State.Attack);
                StartCoroutine(AttackSequence());
                SetState(State.Walk, 1f);
            }
        }
    }

    private void DealDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(DamageTransform.position, DamageRadius, DamageLayer);
        foreach(Collider col in colliders)
        {
            col.gameObject.GetComponent<Damageable>()?.OnDamage(Damage);

        }
    }

    private IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(DamageDelay);
        DealDamage();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(DamageTransform.position, DamageRadius);
    }
}
