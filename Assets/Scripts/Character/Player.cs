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
    //public Transform MyTarget { get; set; } //removed. MyTarget property is now added in Character script


    private IInteractable interactable; //a ref to what the player can interact with

    public int MyGold { get; set; }
    /////// Start is called before the first frame update///////
    protected override void Start()
    {
        MyGold = 20;
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        // Debug.Log(LayerMask.GetMask("Block"));  //��� �� ��� �� layer ��� ����� �� blocks
        //health.MyCurrentValue = 100; //initial health
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);


        base.Update();
    }



    private void GetInput()
    {
        Direction = Vector2.zero; //after every loop reset direction
        ///debugging
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("RUN I");
            health.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("RUN O");

            health.MyCurrentValue += 10;
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

        if (currentTarget != null && InLineOfSight()) //������ �� ������ �� � ������ ����� ���� ��� ������� ���
        {
            Spell spell = Instantiate(spellPrefab[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();
            //spell.MyTarget = currentTarget;
            spell.Initialize(currentTarget, spellDamage, transform); //this is hardcoded spell damage
        }

        StopAttack();

    }

    public void CastSpell(int spellIndex) {
        Block();
        if (MyTarget != null) //���� ��������, ��� target?
        {
            if (!IsAttacking && !isMoving && InLineOfSight()) //����� �� ���� ��� �� lineofsight ����� ���������� bool ����
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