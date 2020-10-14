using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float MaxSpeed;
    public float TurnSpeed = 0.1f;
    private Rigidbody myRigidbody;
    public float jumpForce;
    [Header("References")]
    public Camera MainCamera;
    public Bullet BulletPrefab;
    public Transform Muzzle;
    public Damageable damageable;
    public Animator animator;

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

    private Vector3 aimPosition;

    protected virtual void OnEnable()
    {
        damageable.OnDeath += OnDeath;
        damageable.OnDamageTaken += OnDamageTaken;
    }

    protected virtual void OnDisable()
    {
        damageable.OnDeath -= OnDeath;
        damageable.OnDamageTaken -= OnDamageTaken;
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
        myRigidbody.AddForce(MoveSpeed * currentInput, ForceMode.Acceleration);
        Vector2 planeVelocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.z);
        if (planeVelocity.magnitude > MaxSpeed)
        {
            planeVelocity = planeVelocity.normalized * MaxSpeed;
            myRigidbody.velocity = new Vector3(planeVelocity.x, myRigidbody.velocity.y, planeVelocity.y);
        }

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
                            Input.GetKey(KeyCode.W) ? 1f : 0f + (Input.GetKey(KeyCode.S) ? -1f : 0f)).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        animator.SetFloat("MoveSpeed", myRigidbody.velocity.magnitude);
    }

    private void UpdateRotation()
    {
        // Rotation Raycast
        RaycastHit hit;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, float.MaxValue, AimMask))
        {
            aimPosition = hit.point;
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

    private void OnDamageTaken()
    {
        Debug.Log("Damage Taken: " + damageable.PrintHp());
    }

    private void OnDeath()
    {
        Debug.LogWarning("YOU ARE DEAD!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(aimPosition, 2f);
    // }
}
