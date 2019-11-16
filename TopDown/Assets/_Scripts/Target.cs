using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Damageable
{

    public GameObject HitEffect;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void OnDamage() {
        Destroy(gameObject);
        //Debug.Log("Object destroyed");
    }

    public override GameObject GetHitEffect() {
        return Instantiate(HitEffect);
    }
}
