using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class IdleState : IState
{
    private Enemy parent; //needs ref to parent - Enemy
    public void Enter(Enemy parent)
    {
        this.parent = parent;
        this.parent.Reset();
    }

    public void Exit()
    {
    }

    public void Update()
    {
        //Debug.Log("Enemy Idles");

        if (parent.MyTarget != null) //if i have target then follow //i can do this bc of the getter in Enemy script
        {
            parent.ChangeState(new FollowState());
        }
    }
}
