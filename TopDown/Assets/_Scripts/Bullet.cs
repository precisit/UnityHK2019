using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody myRigidbody;
    public float maxRange;
    Vector3 startPos;
    public GameObject HitEffect;
    RaycastHit hit;
    public LayerMask hitLayers;

    void Start()
    {
        startPos = transform.position;
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.velocity = transform.forward * speed;
    }

    void Update()
    {
        if ((transform.position - startPos).magnitude > maxRange)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.fixedDeltaTime, hitLayers))
        {
            Hit(hit);
        }
    }

    private void Hit(RaycastHit hit)
    {
        Damageable damageable = hit.collider.gameObject.GetComponent<Damageable>();
        GameObject hitEffect = null;
        
        if(damageable) {
            damageable.OnDamage();
            hitEffect = damageable.GetHitEffect();
            hitEffect.transform.position = hit.point;
            hitEffect.transform.rotation = transform.rotation;
        } else {
            hitEffect = Instantiate(HitEffect, hit.point, transform.rotation);
        }
        Destroy(hitEffect, 2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
