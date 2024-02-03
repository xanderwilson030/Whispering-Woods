using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform target;

    public void LookTowardsTarget(Transform tarPos)
    {
        target = tarPos;

        gameObject.transform.up = target.position - transform.position;
    }
}
