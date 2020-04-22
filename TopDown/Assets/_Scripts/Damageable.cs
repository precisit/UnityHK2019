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

    void Awake()
    {
        hp = HpMax;
    }

    public void OnDamage(float damage)
    {
        hp -= damage;
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
}
