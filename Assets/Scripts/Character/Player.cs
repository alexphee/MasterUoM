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
    [SerializeField]
    private Text goldText;
    private float initMana = 100;

    public bool InCombat { get; set; } = false;
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

    //private List<Enemy> attackers = new List<Enemy>();
    //public List<Enemy> MyAttackers { get => attackers; set => attackers = value; }

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
    protected override void Start()
    {
        base.Start();
        StartCoroutine(Regen());
    }
    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        // Debug.Log(LayerMask.GetMask("Block"));  //ãéá íá âñù ôï layer ðïõ Ýâáëá ôá blocks
        //health.MyCurrentValue = 100; //initial health
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);
        //spellBook.GetComponent<SpellBook>();

        goldText.text = MyGold.ToString();

        base.Update();
    }

    public void SetDefaultPlayerValues()
    {
        health.Initialize(initHealth, initHealth);
        MyGold = 20;
        MyMana.Initialize(initMana, initMana);
        MyXP.Initialize(0, Mathf.Floor(100 * MyLevel * Mathf.Pow(MyLevel, 0.5f))); //equation to level up //floor is needed so i get rid of decimal
        levelText.text = MyLevel.ToString();
        goldText.text = MyGold.ToString();
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("GOLD: " + MyGold);
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
        if (newSpell.MyManaCost <= mana.MyCurrentValue) //i need to check if i can actually have enough mana for casting a spell
        {
            Transform currentTarget = MyTarget.MyHitBox; //refactored to access hitbox
            IsAttacking = true;
            MyAnimator.SetBool("attack", IsAttacking); //start attack animation

            yield return new WaitForSeconds(newSpell.MyCastTime); //hardcoded cast time DEBUGGING ONLY //0.3f
            Debug.Log("ATTACK DONE");

            if (currentTarget != null && InLineOfSight()) //ðñÝðåé íá åëÝãîù áí ï å÷èñüò Ýéíáé ðßóù áðü åìðüäéá êëð
            {
                SpellScript spell = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();
                //spell.MyTarget = currentTarget;
                if (spellIndex == 0)
                {
                    spellDamage = newSpell.MyDamage;
                    mana.MyCurrentValue -= newSpell.MyManaCost;
                    spell.Initialize(currentTarget, spellDamage, this); //this is hardcoded spell damage
                }
                else if (spellIndex == 1)
                {
                    spellDamage = newSpell.MyDamage;
                    mana.MyCurrentValue -= newSpell.MyManaCost;
                    spell.Initialize(currentTarget, spellDamage, this);
                }

            }
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
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256); //from player to target 
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

    public void GainGold(int g)
    {
        MyGold += g;
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
        MyMana.MyMaxValue += 10;
        MyMana.MyCurrentValue = MyMana.MyMaxValue;
        MyHealth.MyMaxValue += 10;
        MyHealth.MyMaxValue = MyHealth.MyMaxValue;
        //this is for bug fixxing. When you gain enough XP to go e.g. from 1 to 4
        //i only stops at the second level. Here i check every time if i need to go further
        if (MyXP.MyCurrentValue >= MyXP.MyMaxValue)
        {
            StartCoroutine(LevelUP());
        }
    }

    /*public void AddAttacker(Enemy enemy)
    {
        if (!MyAttackers.Contains(enemy)) //if it doesnt contain enemy then add it
        {
            MyAttackers.Add(enemy);
        }
    }*/

    public void UpdateLevel()
    {
        levelText.text = MyLevel.ToString();
    }

    public void UpdateGold()
    {
        goldText.text = MyGold.ToString();
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
        Direction = Vector2.zero;
        myRigidBody.velocity = Direction;
        //MyInstance.MyTarget = null;
        MySpriteRenderer.enabled = false; //hide
        yield return new WaitForSeconds(3f);
        health.Initialize(initHealth, initHealth);
        MyMana.Initialize(initMana, initMana);
        //this.transform.position = GameObject.FindGameObjectWithTag("Spawn").transform.position;
        gameObject.transform.position = originalPos;
        //transform.position = new Vector2(10f, -10f);
        MySpriteRenderer.enabled = true; //show after respawn
        MyAnimator.SetTrigger("respawn");
    }

    public override void AddAttacker(Character attacker)
    {
        int count = Attackers.Count;
        base.AddAttacker(attacker);
        if (count == 0)
        {
            InCombat = true;
            CombatTextManager.MyInstance.CreateText(transform.position, "ENTER COMBAT", cType.TEXT);
        }
    }

    public override void RemoveAttacker(Character attacker)
    {
        base.RemoveAttacker(attacker);
        if (Attackers.Count == 0)
        {
            InCombat = false;
            CombatTextManager.MyInstance.CreateText(transform.position, "EXIT COMBAT", cType.TEXT);
        }
    }

    private IEnumerator Regen()
    {
        while (true) //keep running as long as game is open
        {
            if (!InCombat) //if not in combat
            {
                if (health.MyCurrentValue < health.MyMaxValue) //if not at max health
                {
                    int value = Mathf.FloorToInt(health.MyMaxValue * 0.05f); //regen 5% of max value
                    health.MyCurrentValue += value;

                    CombatTextManager.MyInstance.CreateText(transform.position, value.ToString(), cType.HEAL);
                }

                yield return new WaitForSeconds(0.5f); //adding a little phase difference so the text isn't on top of each other

                if (mana.MyCurrentValue < mana.MyMaxValue) //if not at max mana
                {
                    int value = Mathf.FloorToInt(mana.MyMaxValue * 0.05f); //regen 5% of max value
                    mana.MyCurrentValue += value;

                    CombatTextManager.MyInstance.CreateText(transform.position, value.ToString(), cType.MANA);
                }

            }
            yield return new WaitForSeconds(2.5f); //rate of regen tick
        }

    }



    public void GetMana(int mana)
    {
        MyMana.MyCurrentValue += mana;
        CombatTextManager.MyInstance.CreateText(transform.position, mana.ToString(), cType.MANA); //write out mana
    }
}