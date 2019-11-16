using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    public abstract void OnDamage();
    public abstract GameObject GetHitEffect();
}
