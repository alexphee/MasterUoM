using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{

    public Character MyTarget { get; set; }
    
    [SerializeField]
    public string type;
    public string MyType { get => type; }

    [SerializeField]
    private float speed;
    //protected Animator MyAnimator; //replaced with property bellow
    public Animator MyAnimator{ get; set; }
    private Vector2 direction;

    protected Rigidbody2D myRigidBody;

    //protected bool IsAttacking = false; //replaced with property bellow
    public bool IsAttacking { get; set; }


    protected Coroutine actionRoutine;

    [SerializeField]
    private Transform hitBox;
    public Transform MyHitBox { get => hitBox; set => hitBox = value; }
    [SerializeField]
    protected Stat health;//////////////θέλω access σε αυτό το health από το UIManager, όμως είναι protected. ’ρα κάνω ένα getter

    public Stat MyHealth { get { return health; } }

    [SerializeField]
    protected float initHealth; // = 100; character's initial health //moved this from player, all players would start with 100 hp but i serialize it
    public bool isMoving
    {
        get { return Direction.x != 0 || Direction.y != 0; } // returns a boolean to isMoving based on the outcome of the condition
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    public bool IsAlive { get { return health.MyCurrentValue > 0; }} //this is going to return true if the health current value is larger than zero and false if less than zero (dead)
    [SerializeField]
    private int level;
    public int MyLevel { get => level; set => level = value; }


    public SpriteRenderer MySpriteRenderer { get; set; }

    public List<Character> Attackers { get; set; } = new List<Character>();

    private Vector2 originalPosition;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        originalPosition = gameObject.transform.position;

        MySpriteRenderer = GetComponent<SpriteRenderer>();
        myRigidBody = GetComponent<Rigidbody2D>();
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }
    public void Move()
    {
        if (IsAlive)
        {
            myRigidBody.velocity = Direction.normalized * Speed; //is player moving?
        }
    }

    public void HandleLayers()
    {
        if(IsAlive)
        {
            if (isMoving)
            { //if player is moving, pley movement animation

                ActivateLayer("WalkLayer"); //set Layer to 1 whenever the function starts
                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);


            }
            else if (IsAttacking) //if player is attacking activate attack layer
            {
                ActivateLayer("AttackLayer");
            }
            else
            {
                ActivateLayer("IdleLayer"); //if no keys pressed go back to idle animations
            }
        }
        else
        {
            ActivateLayer("DeathLayer");
        }
    }

      public void ActivateLayer(string layerName)
    {

            for (int i = 0; i < MyAnimator.layerCount; i++)
            {
                MyAnimator.SetLayerWeight(i, 0);
            }
        
            MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);


    }


    public virtual void TakeDamage(float damage, Character source)
    {
        
       /* if(MyTarget == null)
        {
            MyTarget = source;
        }*/
        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), cType.DAMAGE); //write out health

        if (health.MyCurrentValue <= 0)
        {
            Direction = Vector2.zero; //prevents enemy to continue to move after death while chasing
            myRigidBody.velocity = Direction; //change velocity to zero
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");
            StartCoroutine(RespawnEnemy());

            ///
            ///
            //////TEST -- without this i can push around the enemy after death but if i add it i can't loot :(
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            ///
            //////Stupid solution --> Inspector --> RigidBody2D --> Mass = 99999
        }
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), cType.HEAL); //write out health
    }



    public IEnumerator RespawnEnemy()
    {
        //MySpriteRenderer.enabled = false; //hide
        yield return new WaitForSeconds(180f);
        health.Initialize(initHealth, initHealth);
        gameObject.transform.position = originalPosition;
        //MyTarget = null;
        //MySpriteRenderer.enabled = true; //show after respawn
    }

    public virtual void AddAttacker(Character attacker)
    {
        if (!Attackers.Contains(attacker))
        {
            Attackers.Add(attacker);
        }
    }

    public virtual void RemoveAttacker(Character attacker)
    {
        Attackers.Remove(attacker);
    }
}
