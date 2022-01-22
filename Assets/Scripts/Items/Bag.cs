using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Bag", menuName ="Items/Bag", order =1)] //this command creates a menu so that i can create this asset and set up the serializable fields after
public class Bag : Item, IUseable
{
    [SerializeField]
    private int slots; //each bag will need an amount of slots
    [SerializeField]
    private GameObject bagPrefab; //this prefab is going to instantiate in game, so i can see the bag on screen and click slots etc

    public BagScr MyBagScr { get; set; }
    public BagButton MyBagButton { get; set; }

    public int MySlotCount { get => slots; }
    public void Initialize(int slots) { 
        this.slots = slots;
    }

    public void Use()
    {
        if (InventoryScr.MyInstance.CanAddBag) //use can be executed only if CanAddBag is true, if i only have les than 3 bags
        {
            Remove();
            MyBagScr = Instantiate(bagPrefab, InventoryScr.MyInstance.transform).GetComponent<BagScr>(); //when i right click on bag it creates the bag with 0 slots
            MyBagScr.AddSlots(slots);

            InventoryScr.MyInstance.AddBag(this);//whenever i use the bag i have to call inv
        }
    }
    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nContains 8 slots"); //this is a better way to display health --easier to keep up with changes
    }

    public void SetupScript()
    {
        MyBagScr = Instantiate(bagPrefab, InventoryScr.MyInstance.transform).GetComponent<BagScr>();
        MyBagScr.AddSlots(slots);
    }
}
