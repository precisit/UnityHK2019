using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody myRigidbody;
    public float maxRange;
    Vector3 startPos;
    public GameObject hitEffect;
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
            Hit(hit.point);
        }
    }

    private void Hit(Vector3 point)
    {
        GameObject hit = Instantiate(hitEffect, point, transform.rotation);
        Destroy(hit, 2f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
