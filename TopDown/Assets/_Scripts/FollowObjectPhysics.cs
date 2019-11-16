using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectPhysics : FollowObject
{
    [Header("Physics")]
    public Rigidbody FollowRigidbody;
    public float VelocityOffset;
    public float ZoomPerVelocity;

    private float baseZ;
    private float localOffset;

    protected override void Awake()
    {
        base.Awake();
        baseZ = transform.localPosition.z;
    }

    protected override Vector3 UpdateTargetPosition()
    {
        return base.UpdateTargetPosition() + FollowRigidbody.velocity * VelocityOffset + GetZoom();
    }

    public Vector3 GetZoom()
    {
        return -transform.forward * FollowRigidbody.velocity.magnitude * ZoomPerVelocity;// new Vector3(0f, 0f, baseZ);
    }
}
