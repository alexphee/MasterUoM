using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    protected Animator myAnimator;
    protected Vector2 direction;

    private Rigidbody2D myRigidBody;

    protected bool isAttacking = false;
    protected Coroutine attackRoutine;
    public bool isMoving
    {
        get { return direction.x != 0 || direction.y != 0; } // returns a boolean to isMoving based on the outcome of the condition
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
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
        myRigidBody.velocity = direction.normalized * speed; //is player moving?

    }

    public void HandleLayers()
    {
        if (isMoving)
        { //if player is moving, pley movement animation
            
            ActivateLayer("WalkLayer"); //set Layer to 1 whenever the function starts
            myAnimator.SetFloat("x", direction.x);
            myAnimator.SetFloat("y", direction.y);

            StopAttack(); //stop attack if moving
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

    public void StopAttack()
    {
        // Debug.Log("ATTACK STOP");
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            myAnimator.SetBool("attack", isAttacking);
        }
    }
}
