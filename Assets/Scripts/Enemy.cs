using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    private IState currentState;


    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target; //για να βλέπει το range τον παικτη, για aggro

    public Transform Target { get => target; set => target = value; }

    protected void Awake()
    {
        ChangeState(new IdleState());
    }
    protected override void Update()
    {
        currentState.Update();
        //FollowTarget();  removed
        base.Update();
    }



    public override Transform Select()
    {
        healthGroup.alpha = 1; //makes sure i can see the healthbar when i select the target

        return base.Select();
    }

    public override void Deselect()
    {
        healthGroup.alpha = 0; //hide healthbar
        base.Deselect();
    }

    public override void TakeDamage(float damage) //this overrides Character's TakeDamage function
    {
        base.TakeDamage(damage);
        OnHealthChanged(health.MyCurrentValue); // trigger event after TakeDamage is called, because it reduces the health and updates it. If i trigger the event b4 i update it there is nothing to update. So i change health first and trigger event second, so the UnitFrame get the correct values. else it would be a step behind (at 90 health it would say 100, at 80 --> 90 etc)
    }

    //private void FollowTarget() { }
    

    public void ChangeState(IState newState)
    {
        if(currentState != null) //at first i dont have a state, so this is necessary bc of NullRef exception
        {
            currentState.Exit(); //when i try to change state eg. from idle to attack, first i need to Exit the current state
        }
        currentState = newState; //then the new state becomes the current state
        currentState.Enter(this); //current state needs a ref to self --> this=Enemy
    }
}
