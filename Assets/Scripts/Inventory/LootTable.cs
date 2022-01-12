using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    private Loot[] loot; //loot array

    private List<Item> droppedItems = new List<Item>();

    private bool alreadyRolled = false;
    public void ShowLoot() //when the LootWindow is asked to show the loot it will give the createPages of LootWindow the dropped items
    {
        if (!alreadyRolled)
        {
            Roll();
        }
        LootWindow.MyInstance.CreatePages(droppedItems);
    }
    
    public void Roll() // eg. i put 3 items for the enemy to drop. Each of them has a drop chance on the enemy and i roll sth, if that roll is <= to that drop chance we add it to the items
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if(roll <= item.MyDropChance)
            {
                droppedItems.Add(item.MyItem);
            }
        }
        alreadyRolled = true;
    }
}
