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

    private void Hit()
    {
        GameObject hit = Instantiate(hitEffect, transform.position, transform.rotation);
        Destroy(hit, 2f);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("On Trigger Enter");
        Hit();
        Destroy(gameObject);
    }
}
