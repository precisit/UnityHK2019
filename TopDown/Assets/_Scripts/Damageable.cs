using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public GameObject HitEffect;
    public float HpMax;
    private float hp;

    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;

    public delegate void OnHitDelegate();
    public event OnHitDelegate OnDamageTaken;

    void Awake()
    {
        hp = HpMax;
    }

    public void OnDamage(float damage)
    {
        hp -= damage;
        OnDamageTaken?.Invoke();
        if (hp <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    public GameObject GetHitEffect()
    {
        return Instantiate(HitEffect);
    }

    public string PrintHp()
    {
        return hp + "/" + HpMax;
    }
}
