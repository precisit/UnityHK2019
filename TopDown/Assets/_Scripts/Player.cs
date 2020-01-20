using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MoveSpeed;
    public float TurnSpeed = 0.1f;
    private Rigidbody myRigidbody;
    public Camera MainCamera;
    public LayerMask AimMask;
    public GameObject BulletPrefab;
    public Transform Muzzle;

    private Vector3 currentInput;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }

    private void FixedUpdate()
    {
        myRigidbody.AddForce(MoveSpeed * currentInput, ForceMode.Impulse);
        UpdateRotation();
    }

    private void UpdateMovement()
    {
        // Movement
        currentInput = new Vector3( 
                            (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1f : 0f), 
                            0f, 
                            Input.GetKey(KeyCode.W) ? 1f : 0f + (Input.GetKey(KeyCode.S) ? -1f : 0f));
    }

    private void UpdateRotation()
    {
        // Rotation Raycast
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.MaxValue, AimMask))
        {
            float angle = Vector3.SignedAngle(transform.forward, hit.point.Flatten() - transform.position.Flatten(), Vector3.up);
            myRigidbody.AddTorque(transform.up * angle * TurnSpeed, ForceMode.Force);
        }
    }

    private void UpdateShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(BulletPrefab, Muzzle.transform.position, Muzzle.transform.rotation);
        }
    }
}
