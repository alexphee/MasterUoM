using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    protected Animator myAnimator;
    private Vector2 direction;

    private Rigidbody2D myRigidBody;

    protected bool isAttacking = false;
    protected Coroutine attackRoutine;

    [SerializeField]
    protected Transform hitBox;
    [SerializeField]
    protected Stat health;//////////////θέλω access σε αυτό το health από το UIManager, όμως είναι protected. ’ρα κάνω ένα getter

    public Stat MyHealth { get { return health; } }

    [SerializeField]
    private float initHealth; // = 100; character's initial health //moved this from player, all players would start with 100 hp but i serialize it
    public bool isMoving
    {
        get { return Direction.x != 0 || Direction.y != 0; } // returns a boolean to isMoving based on the outcome of the condition
    }

    public Vector2 Direction { get => direction; set => direction = value; }
    public float Speed { get => speed; set => speed = value; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //moved this from player so everything initializes uptop
        health.Initialize(initHealth, initHealth);

        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
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
        myRigidBody.velocity = Direction.normalized * Speed; //is player moving?

    }

    public void HandleLayers()
    {
        if (isMoving)
        { //if player is moving, pley movement animation
            
            ActivateLayer("WalkLayer"); //set Layer to 1 whenever the function starts
            myAnimator.SetFloat("x", Direction.x);
            myAnimator.SetFloat("y", Direction.y);

            
        }
        else if (isAttacking) //if player is attacking activate attack layer
        {
            ActivateLayer("AttackLayer");
        }else
        {
            ActivateLayer("IdleLayer"); //if no keys pressed go back to idle animations
        }
    }

      public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < myAnimator.layerCount; i++)
        {
            myAnimator.SetLayerWeight(i, 0); //disable all layers
        }
        myAnimator.SetLayerWeight(myAnimator.GetLayerIndex(layerName), 1); //get an index and enable this layer
    }

    

    public virtual void TakeDamage(float damage)
    {
        health.MyCurrentValue -= damage;
        if (health.MyCurrentValue <= 0)
        {
            myAnimator.SetTrigger("die");
        }
    }
}
