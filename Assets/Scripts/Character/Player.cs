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

    /*[SerializeField]
    private GameObject[] spellPrefab;*/ //REMOVED DURING MAJOR SPELL REFACTORING
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

    [SerializeField]
    private Crafting crafting;

    //private SpellBook spellBook;

    public int MyGold { get; set; }

    [SerializeField]
    private Animator levelUp;

    public Stat MyXP { get => xpStat; set => xpStat = value; }
    public Stat MyMana { get => mana; set => mana = value; }

    private List<Enemy> attackers = new List<Enemy>();
    public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }

   // private Vector2 spawnPoint = new Vector2(10f, -10f);
    public Coroutine MyInitRoutine { get; set; } //a routine that initializes sth
    /* protected override void Start() 
     {
         MyGold = 20; /////////EVERYTHING MOVED TO FUNCTION
         MyMana.Initialize(initMana, initMana);
         MyXP.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f))); //equation to level up //floor is needed so i get rid of decimal
         levelText.text = MyLevel.ToString();
         base.Start();
     }*/

    private Vector2 originalPos;
    private void Awake()
    {
        originalPos = gameObject.transform.position;

    }
    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        // Debug.Log(LayerMask.GetMask("Block"));  //ãéá íá âñù ôï layer ðïõ Ýâáëá ôá blocks
        //health.MyCurrentValue = 100; //initial health
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        //spellBook.GetComponent<SpellBook>();


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

        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)))
        {
            exitIndex = 0;
            Direction += Vector2.up;
        }
        if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            exitIndex = 3;
            Direction += Vector2.left;
        }
        if ((Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.DownArrow)))
        {
            exitIndex = 2;
            Direction += Vector2.down;
        }
        if ((Input.GetKey(KeyCode.D)) || (Input.GetKey(KeyCode.RightArrow)))
        {
            exitIndex = 1;
            Direction += Vector2.right;
        }

        if (isMoving)
        {
            StopAction(); //if moving stop casting
        }
    }

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }
    private IEnumerator Attack(int spellIndex)
    {
        Spell newSpell = SpellBook.MyInstance.CastSpelll(spellIndex);
        Transform currentTarget = MyTarget;
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking); //start attack animation

        yield return new WaitForSeconds(newSpell.MyCastTime); //hardcoded cast time DEBUGGING ONLY //0.3f
        Debug.Log("ATTACK DONE");

        if (currentTarget != null && InLineOfSight()) //ðñÝðåé íá åëÝãîù áí ï å÷èñüò Ýéíáé ðßóù áðü åìðüäéá êëð
        {
            SpellScript spell = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
            //spell.MyTarget = currentTarget;
            spell.Initialize(currentTarget, spellDamage, transform); //this is hardcoded spell damage
        }

        StopAction();

    }

    public void CastSpell(int spellIndex)
    {
        Block();
        if (MyTarget != null) //ðñéí óõíå÷ßóù, Ý÷ù target?
        {
            if (!IsAttacking && !isMoving && InLineOfSight()) //ìðïñþ íá âÜëù êáé ôï lineofsight ãéáôß åðéóôñÝöåé bool ôéìÞ
            {
                if (MyTarget.GetComponentInParent<Character>().IsAlive) //this is added later, it prevents casting spells on a dead target
                {
                    actionRoutine = StartCoroutine(Attack(spellIndex));
                }
            }
        }

    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        crafting.AddItemsToInventory();
    }
    private IEnumerator ActionRoutine(ICastable castable)
    {
        IsAttacking = true;
        MyAnimator.SetBool("attack", IsAttacking);
        yield return new WaitForSeconds(castable.MyCastTime);
        StopAction();
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

    public void StopAction()
    {
        SpellBook.MyInstance.StopCasting();//stop casting
        IsAttacking = false; //make sure i dont attack
        MyAnimator.SetBool("attack", IsAttacking); //stop attack animation
        // Debug.Log("ATTACK STOP");
        if (actionRoutine != null) //checks for references to a coroutine
        {
            StopCoroutine(actionRoutine);
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
        CombatTextManager.MyInstance.CreateText(transform.position, xp.ToString(), cType.XP);

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

    public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy)) //if it doesnt contain enemy then add it
        {
            MyAttackers.Add(enemy);
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

    public IEnumerator Respawn()
    {
        StopAction();
        //MyInstance.MyTarget = null;
        MySpriteRenderer.enabled = false; //hide
        yield return new WaitForSeconds(5f);
        health.Initialize(initHealth, initHealth);
        MyMana.Initialize(initMana, initMana);
        //this.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        gameObject.transform.position = originalPos;
        //transform.position = new Vector2(10f, -10f);
        MySpriteRenderer.enabled = true; //show after respawn
        MyAnimator.SetTrigger("respawn");
    }
}