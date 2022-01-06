using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvadeState : IState
{
    private Enemy parent;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
        parent.Direction = Vector2.zero; //when done with the evade state the direction should be zero to stop moving
        parent.Reset(); //resets parent
    }

    public void Update()
    {
        parent.Direction = (parent.MyStartPosition - parent.transform.position).normalized; //calculates the direction bettween the start position and the current position of the enemy

        parent.transform.position = Vector2.MoveTowards(parent.transform.position, parent.MyStartPosition, parent.Speed * Time.deltaTime); //we need to MoveTowards sth, take the current position and move towards the start position, with a speed based on time.deltatime

        float dist = Vector2.Distance(parent.MyStartPosition, parent.transform.position); //distance between start position and target's current position
        if(dist <= 0)
        {
            parent.ChangeState(new IdleState()); //when done evading and run back, go back to idle state
        }
    }
}
