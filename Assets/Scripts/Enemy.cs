using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    public LayerMask targetMask;
    public LayerMask obstructionMask;
    
    // ENCAPSULATION
    public bool playerHit {get; private set;}
    bool isDisabled = false;
    
    void Start()
    {
        StartCoroutine(PointOfViewRoutine());
    }
    // POLYMORPHISM
    public virtual void Attack(){
        playerHit = true;
        isDisabled = true;
        StartCoroutine(DisableEnemy());
    }
    
    void PointOfView()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    moveTo(target.position);
                }
            }
        }
    }

    void moveTo(Vector3 position)
    {   
        // This value will be adapted
        if (Vector3.Distance(transform.position, position) < 2f)
        {
            Attack();
        }
    }

    IEnumerator PointOfViewRoutine()
    {
        while (!isDisabled)
        {
            yield return new WaitForSeconds(0.2f);
            PointOfView();
        }
    }
    IEnumerator DisableEnemy()
    {
        playerHit = false;
        yield return new WaitForSeconds(2);
        isDisabled = false;
    }
}
