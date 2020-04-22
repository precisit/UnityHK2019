using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float TurnSpeed = 0.1f;
    private Rigidbody myRigidbody;
    [Header("References")]
    public Camera MainCamera;
    public Bullet BulletPrefab;
    public Transform Muzzle;
    public Damageable damageable;

    [Header("Damage")]
    public LayerMask AimMask;
    public float Damage;

    [Header("Audio")]
    public AudioClip[] FiringSounds;
    public AudioClip[] FootstepSounds;

    private Vector3 currentInput;
    public AudioSource firingAudioSource;
    public AudioSource footstepAudioSource;
    private float nextAllowedTimeToPlay;

    protected virtual void OnEnable()
    {
        damageable.OnDeath += OnDeath;
    }

    protected virtual void OnDisable()
    {
        damageable.OnDeath -= OnDeath;
    }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
        nextAllowedTimeToPlay = 0;
    }

    private void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }

    private void FixedUpdate()
    {
        myRigidbody.AddForce(MoveSpeed * currentInput, ForceMode.Impulse);

        if(currentInput.x != 0 || currentInput.z != 0) {
            if(Time.time > nextAllowedTimeToPlay && FootstepSounds.Length > 0) {
                footstepAudioSource?.PlayOneShot(FootstepSounds[(int) (Random.value * FootstepSounds.Length)]);
                nextAllowedTimeToPlay = Time.time + 0.3f;
            }
        }
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
            Bullet newBullet = Instantiate(BulletPrefab, Muzzle.transform.position, Muzzle.transform.rotation);
            newBullet.SetDamage(Damage);
            firingAudioSource.PlayOneShot(FiringSounds[(int) (Random.value * FiringSounds.Length)]);
        }
    }

    private void OnDeath()
    {
        Debug.LogWarning("YOU ARE DEAD!");
    }
}
