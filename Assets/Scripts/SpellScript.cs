using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellScript : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Character source;

    [SerializeField]
    private float speed; 

    public Transform MyTarget { get; private set; }

    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>(); // ref to spells rigid body
        //target = GameObject.Find("Target").transform; //DEBUGGING ONLY
    }

    public void Initialize(Transform target, int damage, Character source)
    {
        this.MyTarget = target; //the target i putt in to initialize will be equal to mytarget
        this.damage = damage;
        this.source = source;
    }
    private void FixedUpdate()
    {
        if (MyTarget != null)
        {
            Vector2 direction = MyTarget.position - transform.position; //calculate spells direction
            myRigidBody.velocity = direction.normalized * speed; //move spell using rigid body

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; //calculate rotation angle
             
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //rotate spell to face target
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        //Debug.Log("RUN in OnTriggerEnter2D");
        if (coll.CompareTag("Hitbox") && coll.transform == MyTarget) //δεν μπορω με gameobject γιατι μπερδευεται με το hitbox (που είναι επίσης gameobject) - επίσης θέλω να πετυχαίνω τον στόχο που έχω επιλέξει και όχι κάποιον ίδιου είδους που θα μπει ανάμεσα 
        {
            Character cha = coll.GetComponentInParent<Character>();
            speed = 0;// set projectile speed to zero so it doesnt fly around while playing death animation
            //coll.GetComponentInParent<Enemy>().TakeDamage(damage); //removed line
            cha.TakeDamage(damage, source); //source is the parent of the spell
            GetComponent<Animator>().SetTrigger("impact");
            myRigidBody.velocity = Vector2.zero; //reset velocity when hit sth
            MyTarget = null;

            //Debug.Log("RUN in OnTriggerEnter2D");
        }
    }
}
