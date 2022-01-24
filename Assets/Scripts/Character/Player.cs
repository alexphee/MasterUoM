using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    //[SerializeField]
    //private Stat health; removed this cause i get "Same field nam serialized multiple times" exception. I have a health field in parent Character as well
    [SerializeField]
    private Stat mana;
    [SerializeField]
    private Stat xpStat;
    [SerializeField]
    private Text levelText;
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
    //public Transform MyTarget { get; set; } //removed. MyTarget property is now added in Character script


    private IInteractable interactable; //a ref to what the player can interact with

    public int MyGold { get; set; }

    [SerializeField]
    private Animator levelUp;

    public Stat MyXP { get => xpStat; set => xpStat = value; }
    public Stat MyMana { get => mana; set => mana = value; }
   
   /* protected override void Start() /////////MOVED TO FUNCTION
    {
        MyGold = 20;
        MyMana.Initialize(initMana, initMana);
        MyXP.Initialize(0, Mathf.Floor( 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f))); //equation to level up //floor is needed so i get rid of decimal
        levelText.text = MyLevel.ToString();
        base.Start();
    }*/

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        // Debug.Log(LayerMask.GetMask("Block"));  //για να βρω το layer που έβαλα τα blocks
        //health.MyCurrentValue = 100; //initial health
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);


        base.Update();
    }

    public void SetDefaultPlayerValues()
    {
        health.Initialize(initHealth, initHealth);
        MyGold = 20;
        MyMana.Initialize(initMana, initMana);
        MyXP.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f))); //equation to level up //floor is needed so i get rid of decimal
        levelText.text = MyLevel.ToString();
    }

    private void GetInput()
    {
        Direction = Vector2.zero; //after every loop reset direction
        ///debugging
        if (Input.GetKeyDown(KeyCode.I))
        {
            //Debug.Log("RUN I");
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 5;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //Debug.Log("RUN O");

            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 5;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            GainExperience(1000);
        }

        if (Input.GetKey(KeyCode.W)) {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }

        if (isMoving)
        {
            StopAttack(); //if moving stop casting
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
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking); //start attack animation

        yield return new WaitForSeconds(1); //hardcoded cast time DEBUGGING ONLY //0.3f
        Debug.Log("ATTACK DONE");

        if (currentTarget != null && InLineOfSight()) //πρέπει να ελέγξω αν ο εχθρός έιναι πίσω από εμπόδια κλπ
        {
            Spell spell = Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();
            //spell.MyTarget = currentTarget;
            spell.Initialize(currentTarget, spellDamage, transform); //this is hardcoded spell damage
        }

        StopAttack();

    }

    public void CastSpell(int spellIndex) {
        Block();
        if (MyTarget != null) //πριν συνεχίσω, έχω target?
        {
            if (!IsAttacking && !isMoving && InLineOfSight()) //μπορώ να βάλω και το lineofsight γιατί επιστρέφει bool τιμή
            {
                if (MyTarget.GetComponentInParent<Character>().IsAlive) //this is added later, it prevents casting spells on a dead target
                {
                    attackRoutine = StartCoroutine(Attack(spellIndex));
                }
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

    public void StopAttack()
    {
        IsAttacking = false; //make sure i dont attack
        MyAnimator.SetBool("attack", IsAttacking); //stop attack animation
        // Debug.Log("ATTACK STOP");
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);

        }
    }

    ////singleton test
    private static Player instance;
    public static Player MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
            }
            return instance;
        }
    }


    public void Interact()
    {
        if (interactable != null)
        {
            interactable.Interact(); //this calls the given interact function eg Enemy's or chest's
        }
    }

    public void GainExperience(int xp)
    {
        MyXP.MyCurrentValue += xp; //add xp to currentvalue
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), cType.XP) ;

        if (MyXP.MyCurrentValue >= MyXP.MyMaxValue)
        {
            StartCoroutine(LevelUP());
        }
    }

    public IEnumerator LevelUP()
    {
        while (!MyXP.IsXPFull) //checks if the bar is full. In case of slow fill speed it will wait longer
        {
            yield return null;
        }
        MyLevel++; //add 1 to level
        levelUp.SetTrigger("levelUp");
        levelText.text = MyLevel.ToString();
        MyXP.MyMaxValue = 100 * MyLevel * Mathf.Pow(MyLevel, 0.5f); //mylevel is used in this equation so it returns sth else for every level, hence the maxvalue will be different everytime
        MyXP.MyMaxValue = Mathf.Floor(MyXP.MyMaxValue); //round this down so i dont get decimal
        MyXP.MyCurrentValue = MyXP.MyOverflow; //when reseting for the new level i need to pass to the bar the ammount of extra xp i had e.g i have 95/100 and gain 10 xp, i should be able to carry the extra 5xp to the new level and not lose it
        MyXP.Reset();

        //this is for bug fixxing. When you gain enough XP to go e.g. from 1 to 4
        //i only stops at the second level. Here i check every time if i need to go further
        if (MyXP.MyCurrentValue >= MyXP.MyMaxValue)
        {
            StartCoroutine(LevelUP());
        }
    }

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("thyEnemy") || collision.CompareTag("Interactable")) //if collide with enemy then its interactable or with a chest
        {
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("thyEnemy") || collision.CompareTag("Interactable")) //if collide with enemy then its interactable or with a chest
        {
            if (interactable != null) //saves me from NullRefExc
            {
                interactable.StopInteraction(); //if it is a chest, close it // if enemy close lootwindow
                interactable = null;
            }
           
        }
    }
}
