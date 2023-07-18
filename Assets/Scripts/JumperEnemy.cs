using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperEnemy : Enemy
{
    float jumpForce = 5000f;
    float velocity = 100f;
    Rigidbody enemyRigidbody;
    public override float Radius 
    { 
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
        StartCoroutine(PointOfViewRoutine());
        enemyRigidbody = GetComponent<Rigidbody>();
        
        Radius = 8f;
        Angle = 160f;
    }
    public override void moveTo(Vector3 position)
    {
        Vector3 playerDirection = (position - transform.position).normalized;
        
        enemyRigidbody.AddForce(Vector3.up * jumpForce);
        enemyRigidbody.AddForce(playerDirection * velocity, ForceMode.Impulse);
    }
}
