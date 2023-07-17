using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    bool isDisabled = false;
    float radius = 4.7f;
    [Range(0, 360)]
    float angle = 48f;
    float velocity = 120f;
    float disabledTime = 2;    
    List<Material> materialList = new List<Material>(1);
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstructionMask;   
    public bool playerHit;
    public Rigidbody enemyRigidbody;
    public Material[] enemyMaterials;
    public MeshRenderer currentMaterial;    

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        enemyMaterials = GetComponent<Renderer>().materials;
        currentMaterial = GetComponent<MeshRenderer>();
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
        Vector3 playerDirection = (position - transform.position).normalized;
        
        enemyRigidbody.AddForce(playerDirection * velocity, ForceMode.Impulse);
        
        StartCoroutine(DisableEnemyRoutine());
    }
    void DisableEnemy()
    {
        StopCoroutine(PointOfViewRoutine());
        isDisabled = true;        
        materialList.Add(enemyMaterials[1]);        
        currentMaterial.SetMaterials(materialList);
    }
    void EnableEnemy()
    {
        isDisabled = false;
        materialList[0] = enemyMaterials[0];
        currentMaterial.SetMaterials(materialList);
        materialList.Clear();
        StartCoroutine(PointOfViewRoutine());
    }
    public IEnumerator PointOfViewRoutine()
    {
        while (!isDisabled)
        {
            yield return new WaitForSeconds(0.2f);
            PointOfView();
        }
    }
    IEnumerator DisableEnemyRoutine()
    {   
        DisableEnemy();
        
        yield return new WaitForSeconds(disabledTime);

        EnableEnemy();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            // Could be set to false after in the game manager
            playerHit = true;
        }
    }
}
