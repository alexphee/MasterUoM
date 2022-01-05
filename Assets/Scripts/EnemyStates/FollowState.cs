﻿using System;
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
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero;
    }

    public void Update()
    {
        if (parent.Target != null) //necessary check bc when i outrun enemy he keeps walking to the direction i was
        {
            parent.Direction = (parent.Target.transform.position - parent.transform.position).normalized; //find targets direction
            parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.Target.position, parent.Speed * Time.deltaTime); //start moving enemy to targets direction
        }
        else { parent.ChangeState(new IdleState()); } //if enemy has no target, then go back to idle --Exit() vector2.zero
    }
}
