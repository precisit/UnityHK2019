using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : Enemy
{
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
                    agent.speed = moveSpeed;
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

    public override void OnDamage()
    {
        base.OnDamage();
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
                SetState(State.Walk, 1f);
            }
        }
    }
}
