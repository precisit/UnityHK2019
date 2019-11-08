using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    private Rigidbody myRigidbody;
    public Camera mainCamera;
    public LayerMask mask;
    public GameObject bulletPrefab;
    public Transform muzzle;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateRotation();

        UpdateShooting();
    }

    private void UpdateMovement()
    {
        // Movement
        Vector3 input = new Vector3( 
                            (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1f : 0f), 
                            0f, 
                            Input.GetKey(KeyCode.W) ? 1f : 0f + (Input.GetKey(KeyCode.S) ? -1f : 0f));
        myRigidbody.AddForce(moveSpeed * input, ForceMode.Impulse);
    }

    private void UpdateRotation()
    {
        // Rotation Raycast
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.MaxValue, mask))
        {
            Vector3 direction = Utils.Flatten(hit.point) - Utils.Flatten(transform.position);
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

    private void UpdateShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation);
        }
    }
}
