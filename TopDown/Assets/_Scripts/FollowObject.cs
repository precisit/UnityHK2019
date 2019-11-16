using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public enum LookAtType{
        None,
        AtStart,
        Continuous
    };
    public GameObject Follow;
    public Vector3 Offset;
    public LookAtType LookAt;
    [Range(0f, 1f)]
    public float Smoothing;

    protected Vector3 targetPosition;
    protected Vector3 currentVelocity;

    protected virtual void Awake()
    {
        currentVelocity = Vector3.zero;
    }

    protected void Start()
    {
        if (LookAt == LookAtType.AtStart){
            transform.rotation = GetRotation();
        }
    }

    void FixedUpdate()
    {
        targetPosition = UpdateTargetPosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, Smoothing);
        if (LookAt == LookAtType.Continuous){
            transform.rotation = GetRotation();
        }
    }

    protected Quaternion GetRotation()
    {
        return Quaternion.LookRotation((Follow.transform.position - transform.position), Vector3.up);
    }

    protected virtual Vector3 UpdateTargetPosition()
    {
        return Follow.transform.position + Offset;
    }
}
