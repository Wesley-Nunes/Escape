using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderEnemy : Enemy
{
    void Start()
    {
        StartCoroutine(PointOfViewRoutine());
    }
}
