using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    private Enemy parent;

    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
    }


    public void Update()
    {
        //Debug.Log("Enemy Attack");

        if (parent.Target != null)
        {
            float distance = Vector2.Distance(parent.Target.position, parent.transform.position);
            if(distance >= parent.MyAttackRange) //if player gets out of range enemy cant attack and only follows
            {
                parent.ChangeState(new FollowState());
            }
            //check range and attack
        }
        else
        {
            parent.ChangeState(new IdleState());
        }
    }
}
