using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScr : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab; //the bag has to create slots so it needs a slot prefab. Serialize it so it accesses a prefab and instantiate it

    public void AddSlots(int slotCount) //the amount of slots the bag script is going to create
    {
        for (int i = 0; i < slotCount; i++) 
        {
            Instantiate(slotPrefab, transform); //here i instantiate the slot prefab and with transform im putting it as child object of bagscript so the slots will be childs of the bag
        }
    }
}
