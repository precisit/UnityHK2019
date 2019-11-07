using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public GameObject Follow;
    public Vector3 Offset;
    public bool LookAt;

    void LateUpdate()
    {
        transform.position = Follow.transform.position + Offset;
        if (LookAt){
            transform.rotation = Quaternion.LookRotation((Follow.transform.position - transform.position), Vector3.up);
        }
    }
}
