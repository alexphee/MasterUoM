using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);
public delegate void NPCRemoved();
public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;
    public event NPCRemoved npcRemoved;
    [SerializeField]
    private Sprite portraitFace;
    public Sprite ThePortraitFace { get { return portraitFace; } }



    [SerializeField]
    private LootTable loottable;

    private IState currentState;
    [SerializeField]
    private float attackRange;
    //public float MyAttackRange { get; set; }
    public  float MyAttackTime { get; set; }

    public Vector3 MyStartPosition { get; set; }

    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    public float initialAggroRange; //the standard aggro range
    public float MyAggroRange{ get; set; }//actual aggro range is based on the distance tha the enemy gonna attack from

    public bool Inrange { get { return Vector2.Distance(transform.position, MyTarget.transform.position) < MyAggroRange; } }

    public float MyAttackRange { get => attackRange; set => attackRange = value; }

    //private Transform target; //για να βλέπει το range τον παικτη, για aggro
    //public Transform Target { get => target; set => target = value; }  // those 2 lines were removed. MyTarget property is now added in Character script


    [SerializeField]
    protected int damage;

    protected void Awake()
    {
        health.Initialize(initHealth, initHealth);
        MyStartPosition = transform.position; //the start position is set by the transform position in the beggining of the game so the enemy knows whereit needs to reset to
        MyAggroRange = initialAggroRange; //the starting aggro range. This is going to change based on the distance the player attacks from
        //MyAttackRange = 1; //hardcoded -- serialized it
        ChangeState(new IdleState());
    }
    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking) //as long as im not attacking, im counting how long since last attack
            {
                MyAttackTime += Time.deltaTime; //the reason of this if statement is if i have attack interval like 3sec and my attack takes 1 sec i need to exclude this from the actual cooldown of 3sec. This ensures the time i put as cooldown time for attack is correct 
            }

            currentState.Update();
            if (MyTarget != null && !Player.MyInstance.IsAlive)
            {
                ChangeState(new EvadeState());//when player dies, enemy evades back to original position
            }
            //FollowTarget();  removed
        }            
        base.Update();

    }



    public Character Select()
    {
        healthGroup.alpha = 1; //makes sure i can see the healthbar when i select the target

        return this;
    }

    public void Deselect()
    {
        healthGroup.alpha = 0; //hide healthbar
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        npcRemoved -= new NPCRemoved(UIManager.MyInstance.HideTargetframe);
    }


    public override void TakeDamage(float damage, Character source) //this overrides Character's TakeDamage function //source is later added, as the source of incoming damage
    {
        if (!(currentState is EvadeState))//if current state is not the evade state, then is allowed to take dmg
        {
            if (IsAlive)
            {
                SetTarget(source); //feed SetTarget with the source of damage.
                base.TakeDamage(damage, source);
                OnHealthChanged(health.MyCurrentValue); // trigger event after TakeDamage is called, because it reduces the health and updates it. If i trigger the event b4 i update it there is nothing to update. So i change health first and trigger event second, so the UnitFrame get the correct values. else it would be a step behind (at 90 health it would say 100, at 80 --> 90 etc)

                if (!IsAlive)
                {
                    //Player.MyInstance.MyAttackers.Remove(this);
                    source.RemoveAttacker(this); //if not alive remove from attackers list
                    Player.MyInstance.GainExperience(XPManager.CalculateXP(this as Enemy)); //if whatever died was an enemy then player needs to gain XP // this prevents gaining XP from player deaths
                }
            }
        }
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

    public void SetTarget(Character target) //feed SetTarget a target
    {
        if (MyTarget == null && !(currentState is EvadeState)) //if there is no target and target is not in evade state
        {
            float distance = Vector2.Distance(transform.position, target.transform.position); //calculate distance between target and enemy
            MyAggroRange = initialAggroRange;
            MyAggroRange += distance;
            MyTarget = target;
            target.AddAttacker(this);
        }
    }

    public void Reset()
    {
        MyTarget = null; //reset target
        this.MyAggroRange = initialAggroRange; //resets the aggrorange
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue; //resets actuall health
        OnHealthChanged(health.MyCurrentValue); //reset health on unit frame
    }

    public void Interact()
    {
        if (!IsAlive)
        {
            loottable.ShowLoot();       
        }
    }

    public void StopInteraction()
    {
        LootWindow.MyInstance.Close(); //when i stop interacting with the enemy i have to close the loot window
        //base.StopInteraction();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null) //good practice in events. Always check if something is actually "listening", else i get NullRef exception
        {
            healthChanged(health);
        }
    }

    public void OnNPCRemoved()
    {
        if (npcRemoved != null) //maybe this is better, instead of if statement:  npcRemoved?.Invoke();
        {
            npcRemoved();
        }
        Destroy(gameObject);
    }



    public virtual void DoDmg()
    {
        MyTarget.TakeDamage(damage, this);
           //Player.MyInstance.TakeDamage(damage, transform); //this is "hardcoded" dmg for the player. Even if the enemy attacks sth else, player will take dmg
        
    }
}
