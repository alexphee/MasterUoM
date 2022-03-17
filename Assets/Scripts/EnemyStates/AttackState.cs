using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public Enemy parent;
    private float atkCooldown = 1;

    private float moreRange = 0.1f;
    public void Enter(Enemy parent)
    {
        this.parent = parent;
    }

    public void Exit()
    {
    }


    public void Update()
    {
       
        if (parent.MyAttackTime >= atkCooldown && !parent.IsAttacking) //attack cooldown
        {
            parent.MyAttackTime = 0; //reset time
            parent.StartCoroutine(Attack()); //i need the parent to start a coroutine bc in unity you need a monobehaviour to start a coroutine, the parent is enemy which is a monobehaviour and i can use this for the coroutine
        }

        if (parent.MyTarget != null)
        {
            float distance = Vector2.Distance(parent.MyTarget.transform.position, parent.transform.position);
            if (distance >= parent.MyAttackRange + moreRange && !parent.IsAttacking) //if player gets out of range enemy cant attack and only follows
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

    public IEnumerator Attack()
    {
        //Debug.Log("Enemy Attack");
        parent.IsAttacking = true;
        parent.MyAnimator.SetTrigger("attack");
        //need to wait or else attack is not shown
        parent.DoDmg();
        yield return new WaitForSeconds(parent.MyAnimator.GetCurrentAnimatorStateInfo(2).length); //get info of the current clip that is playing. (2) refers to the layer
        parent.IsAttacking = false;

    }
}
