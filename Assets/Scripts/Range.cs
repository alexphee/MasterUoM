using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Enemy parent;
    private void Start()
    {
        parent = GetComponentInParent<Enemy>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            //parent.SetTarget(collision.transform); //set target
            parent.SetTarget(collision.GetComponent<Character>()); //refactored
        }
    }

    //old way of detargeting the player
   /* private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parent.MyTarget = null;
        }
    }*/
}
