using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class CylinderEnemy : Enemy
{ 
    float velocity = 120f;
    Rigidbody enemyRigidbody;
    // POLYMORPHISM
    public override float Radius 
    {   // ENCAPSULATION
        get { return radius; }
        set { radius = value; } 
    }
    public override float Angle 
    {
        get { return angle; }
        set { angle = value; }
    }
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        StartCoroutine(PointOfViewRoutine());
        
        Radius = 4.7f;
        Angle = 48f;
    }
    public override void moveTo(Vector3 position)
    {
        Vector3 playerDirection = (position - transform.position).normalized;
        
        enemyRigidbody.AddForce(playerDirection * velocity, ForceMode.Impulse);
    }
}
