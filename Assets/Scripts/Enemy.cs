using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    private Transform target; //για να βλέπει το range τον παικτη, για aggro

    public Transform Target { get => target; set => target = value; }

    protected override void Update()
    {
        FollowTarget();
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

    private void FollowTarget()
    {
        if(target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        else
        {
            direction = Vector2.zero; //enemy idles when aggro is lost (when out of range)
        }
    }
}
