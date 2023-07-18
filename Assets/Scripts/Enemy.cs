using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    bool isDisabled = false;
    protected float radius;
    [Range(0, 360)]
    protected float angle;
    public abstract float Radius{ get; set; }
    public abstract float Angle{ get; set; }
    float disabledTime = 2;    
    List<Material> materialList = new List<Material>(1);
    [SerializeField]
    LayerMask targetMask;
    [SerializeField]
    LayerMask obstructionMask;   
    public bool playerHit {get; private set;}
    public Material[] enemyMaterials = new Material[2];
    public MeshRenderer currentMaterial;
    void Start()
    {
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
                    StartCoroutine(ResetEnemyRoutine());
                }
            }
        }
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
    IEnumerator ResetEnemyRoutine()
    {   
        DisableEnemy();
        
        yield return new WaitForSeconds(disabledTime);

        EnableEnemy();
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            playerHit = true;
        }
    }
    public IEnumerator PointOfViewRoutine()
    {
        while (!isDisabled)
        {
            yield return new WaitForSeconds(0.2f);
            PointOfView();
        }
    }
    public virtual void moveTo(Vector3 position) {}
    public void resetPlayerHitState()
    {
        playerHit = false;
    }
}
