using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat health;
    [SerializeField]
    private Stat mana;

    private float initHealth = 100;
    private float initMana = 50;

    [SerializeField]
    private GameObject[] spellPrefab;
    [SerializeField]
    private Transform[] exitPoints;
    [SerializeField]
    private Block[] blocks;
    private int exitIndex = 2; //just to make sure i use the correct direction // 2 is down, player starts facing down

    private Transform target;

    public Transform MyTarget { get; set; }

    /////// Start is called before the first frame update///////
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
       // Debug.Log(LayerMask.GetMask("Block"));  //για να βρω το layer που έβαλα τα blocks
        //health.MyCurrentValue = 100; //initial health

        base.Update();
    }

   

    private void GetInput()
    {
        direction = Vector2.zero; //after every loop reset direction
        ///debugging start
        /*if (Input.GetKeyDown(KeyCode.I)){
            Debug.Log("RUN I");
            health.MyCurrentValue -= 10; 
        }
        if (Input.GetKeyDown(KeyCode.O)){
            Debug.Log("RUN O");

            health.MyCurrentValue += 10; 
        }*/

        if (Input.GetKey(KeyCode.W)) {
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }
       
    }

    private IEnumerator Attack(int spellIndex)
    {
       
            isAttacking = true;
            myAnimator.SetBool("attack", isAttacking);

            yield return new WaitForSeconds(0.3f); //hardcoded cast time DEBUGGING ONLY
            Debug.Log("ATTACK DONE");

            Spell spell = Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();
        spell.MyTarget = MyTarget;
        StopAttack();
        
    }

    public void CastSpell(int spellIndex) {
        Block();
        if (MyTarget != null) //πριν συνεχίσω, έχω target?
        {
            if (!isAttacking && !isMoving && InLineOfSight()) //μπορώ να βάλω και το lineofsight γιατί επιστρέφει bool τιμή
            {
                attackRoutine = StartCoroutine(Attack(spellIndex));
            }
        }
       
    }

    private bool InLineOfSight()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
        //Debug.DrawRay(transform.position, targetDirection, Color.red);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.position),256); //from player to target 
        if (hit.collider == null) { //if dont hit anything
            return true;
        }
        
        return false;
    }

    private void Block() //block view
    {
        foreach (Block b in blocks) // im looking for the class Block inside the array blocks
            b.Deactivate();

        blocks[exitIndex].Activate();
    }
}
