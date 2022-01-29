using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject leafPrefab;
    [SerializeField]
    private Transform exitPoint;

    
    /*public void Shoot(int exitIndex)
    {
        Debug.Log("shoot");
        SpellScript s = Instantiate(leafPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(MyTarget, damage, transform);
    }*/
    public override void DoDmg()
    {
        Debug.Log("shoot");
        SpellScript s = Instantiate(leafPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(MyTarget, damage, transform);
        base.DoDmg();
    }
}
