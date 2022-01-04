using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    //[SerializeField]
    //private Stat health; removed this cause i get "Same field nam serialized multiple times" exception. I have a health field in parent Character as well
    [SerializeField]
    private Stat mana;


    private float initMana = 50;

    [SerializeField]
    private GameObject[] spellPrefab;
    [SerializeField]
    private Transform[] exitPoints;
    [SerializeField]
    private Block[] blocks;
    private int exitIndex = 2; //just to make sure i use the correct direction // 2 is down, player starts facing down

    private Transform target;

    private int spellDamage = 10;

    private Vector3 min, max;
    public Transform MyTarget { get; set; }

    /////// Start is called before the first frame update///////
    protected override void Start()
    {
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        // Debug.Log(LayerMask.GetMask("Block"));  //για να βρω το layer που έβαλα τα blocks
        //health.MyCurrentValue = 100; //initial health
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);


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

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;
            isAttacking = true;
            myAnimator.SetBool("attack", isAttacking); //start attack animation

            yield return new WaitForSeconds(1); //hardcoded cast time DEBUGGING ONLY //0.3f
             Debug.Log("ATTACK DONE");
            
        if (currentTarget != null && InLineOfSight()) //πρέπει να ελέγξω αν ο εχθρός έιναι πίσω από εμπόδια κλπ
        {
            Spell spell = Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();
            //spell.MyTarget = currentTarget;
            spell.Initialize(currentTarget, spellDamage); //this is hardcoded spell damage
        }
            
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
        if (MyTarget != null) //necessary check. else if i click enemy, attack him and instantlly click somewhere to deselect enemy, i get null reference exception. With this it plays animation but doesnt attack
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
            //Debug.DrawRay(transform.position, targetDirection, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.position), 256); //from player to target 
            if (hit.collider == null)
            { //if dont hit anything
                return true;
            }
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
