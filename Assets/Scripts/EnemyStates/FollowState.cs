using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FollowState : IState
{
    private Enemy parent; //needs ref to parent - Enemy
    public void Enter(Enemy parent)
    {
        Player.MyInstance.AddAttacker(parent);
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        //Debug.Log("Enemy Follows");

        if (parent.MyTarget != null) //necessary check bc when i outrun enemy he keeps walking to the direction i was
        {
            parent.Direction = (parent.MyTarget.transform.position - parent.transform.position).normalized; //find targets direction
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyTarget.position, parent.Speed * Time.deltaTime); //start moving enemy to targets direction

            float distance = Vector2.Distance(parent.MyTarget.transform.position, parent.transform.position);
            if(distance <= parent.MyAttackRange) //if close enough to attack then change state to attack state
            {
                parent.ChangeState(new AttackState());
            }
        }
        if(!parent.Inrange) { 
            parent.ChangeState(new EvadeState()); //if enemy has no target, then go to evade state and then back to idle --Exit() vector2.zero
        } 
    }
}
