using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderEnemy : Enemy
{
    float velocity = 120f;
    Rigidbody enemyRigidbody;
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        StartCoroutine(PointOfViewRoutine());
    }
    public override void moveTo(Vector3 position)
    {
        Vector3 playerDirection = (position - transform.position).normalized;
        
        enemyRigidbody.AddForce(playerDirection * velocity, ForceMode.Impulse);
    }
}
