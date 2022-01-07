using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag", menuName ="Items/Bag", order =1)] //this command creates a menu so that i can create this asset and set up the serializable fields after
public class Bag : Item, IUseable
{
    private int slots; //each bag will need an amount of slots
    [SerializeField]
    private GameObject bagPrefab; //this prefab is going to instantiate in game, so i can see the bag on screen and click slots etc

    public BagScr MyBagScr { get; set; }
    public int Slots { get => slots; }
    public void Initialize(int slots) { 
        this.slots = slots;
    }

    public void Use()
    {
        MyBagScr = Instantiate(bagPrefab, InventoryScr.MyInstance.transform).GetComponent<BagScr>(); //when i right click on bag it creates the bag with 0 slots
        MyBagScr.AddSlots(slots);
    }
}
