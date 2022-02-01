using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField]
    private GameObject leafPrefab;
    [SerializeField]
    private Transform exitPoint;

    private float fieldOfView = 100;

    protected override void Update()
    {
        LookTarget();
        base.Update();  
    }
    /*    public void Shoot(int exitIndex)
        {
            Debug.Log("shoot");
            SpellScript s = Instantiate(leafPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
            s.Initialize(MyTarget.MyHitBox, damage, this);

        }*/
    public override void DoDmg()
    {
        Debug.Log("shoot");
        SpellScript s = Instantiate(leafPrefab, exitPoint.position, Quaternion.identity).GetComponent<SpellScript>();
        s.Initialize(MyTarget.MyHitBox, damage, this);
        base.DoDmg();
    }

    private void LookTarget()
    {
        if (MyTarget != null)
        {
            Vector2 directionToTarget = (MyTarget.transform.position - transform.position).normalized;
            Vector2 faceing = new Vector2(MyAnimator.GetFloat("x"), MyAnimator.GetFloat("y"));
            float angleToTarget = Vector2.Angle(faceing, directionToTarget);
            if (angleToTarget > fieldOfView/2)
            {
                MyAnimator.SetFloat("x", directionToTarget.x);
                MyAnimator.SetFloat("y", directionToTarget.y);
            }
        }
    }
}
