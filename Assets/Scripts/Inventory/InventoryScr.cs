using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ItemCountChanged(Item item); //TEST TEST
public class InventoryScr : MonoBehaviour
{
    public event ItemCountChanged itemCountChangedEvent; //TES TEST each time i pick an item

    private static InventoryScr instance;
    public static InventoryScr MyInstance
    {
        //LOGIC: When i access the UIManager through MyInstance we check if the instance is set and if is not i set it and return it
        //Because of the structure i only have one instance of the game, i always return the same instance
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScr>();
            }

            return instance;
        }
    }

    private SlotScr fromSlot; //need ref to whatever i carry around. This is going to be used whenever i click on sth to move it somewhere else, the fromSlot is the slot the item is coming from, when we place it somewhere else in inv
    private List<Bag> bags = new List<Bag>();
    public bool CanAddBag { get { return MyBags.Count < 1; } }

    public int MyEmptySlotCount
    { //everytime i look at a bag i look at all slots and count them together and return them
        get
        {
            int count = 0;
            foreach (Bag bag in MyBags)
            {
                count += bag.MyBagScr.MyEmptySlotCount;
            }
            return count;
        }
    }
    public SlotScr FromSlot
    {
        get => fromSlot;
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.gray; //if i click on any item in my inv this code is going to set the fromSlot equal to whatever i click on and thenm im graying it out
            }
        }
    }

    public List<Bag> MyBags { get => bags; set => bags = value; }

    [SerializeField]
    private BagButton[] bagbuttons;


    [SerializeField]
    public Item[] items; //DEBBUGING PURPOSES
    private void Awake()
    {
        Bag bag =  (Bag)Instantiate(items[0]); //create bag -- casting, bad practice
        bag.Initialize(28); //initialize bag
        bag.Use(); //use bag
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.J))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            bag.Use();
        }*/


        //////DEBUG
       /* if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            AddItem(bag);
        }*/

       /* if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Element carbon = (Element)Instantiate(items[2]);
            AddItem(carbon);
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Element oxygen = (Element)Instantiate(items[3]);
            AddItem(oxygen);
        }*/
        

    }
    
    public void AddBag(Bag bag) //takes the bag we are equiping
    {
        foreach (BagButton bagButton in bagbuttons)
        {
            if (bagButton.MyBag == null) //if i dont have a bag already
            {
                bagButton.MyBag = bag; //then assign a bag
                MyBags.Add(bag); //keep track of bag counter so it doesnt overfloat
                bag.MyBagButton = bagButton; //the bag itself has a ref to the button its sitting on, by creating that ref im able to use it whenever i remove the bag
                break; //dont do this for every bag, just one
            }
        }
    }
    //overload method
    public void AddBag(Bag bag, int bagIndex)
    {
        bag.SetupScript();
        MyBags.Add(bag);
        bag.MyBagScr.MyBagIndex = bagIndex; //BUGFIX
        bag.MyBagButton = bagbuttons[bagIndex];
        bagbuttons[bagIndex].MyBag = bag;
    }
    public void RemoveBag(Bag bag)
    {
        MyBags.Remove(bag); //take the specific bag from bags list uptop 
        Destroy(bag.MyBagScr.gameObject);
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item))
            {
                return true;
            }
        }
        return PlaceInEmpty(item); //if you cant place it anywhere, place it empty

        //old code
        /*foreach (Bag bag in bags) //checks all bags
        {
            if (bag.MyBagScr.AddItem(item))
            {
                return;
            }
        }*/
    }

    private bool PlaceInEmpty(Item item) //place item in empty slot
    {
        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScr.AddItem(item))
            {
                //Debug.Log("Item added !");
                OnItemCountChanged(item); //TEST TEST
                return true;
            }
        }
        return false;
    }
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in MyBags) //check all bags
        {
            foreach (SlotScr slotScr in bag.MyBagScr.MySlots) //check all slots in bag
            {
                if (slotScr.StackItem(item)) //check if i can stack the item on some slot
                {
                    OnItemCountChanged(item); //TEST TEST
                    return true;
                }
            }
        }
        return false;
    }

    public void OpenClose()
    {
        bool closedBag = MyBags.Find(x => !x.MyBagScr.IsOpen); //loop through all bags in list and if a single bag is closed then set closedBag to true. So if closedBag is true i know at least one bag is closed
        //if closed bag == true then open all closed bags
        //if closed bag == false then close all open bags
        foreach (Bag bag in MyBags)
        {
            if (bag.MyBagScr.IsOpen != closedBag)
            {
                bag.MyBagScr.OpenClose();
            }
        }
    }

    public int GetItemCount(string type)
    {
        int itemCount = 0;
        foreach (Bag bag in MyBags) //run through all bags
        {
            foreach (SlotScr slot in bag.MyBagScr.MySlots)//run through all slots in the bag
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type) //if the specific slot is not empty and the item that has on it is the same type im looking for
                {
                    itemCount += slot.MyItems.Count; //add to the item count how many items i have
                }
            }
        }
        return itemCount;
    }

    public Stack<Item> GetItems(string type, int count) // returns a stack of items based on a number e.g. 5 Carbon //if i have 10 i only gonna need the 5 
    {
        Stack<Item> items = new Stack<Item>(); //create stack of items
        foreach (Bag bag in MyBags) //run through all bags
        {
            foreach (SlotScr slot in bag.MyBagScr.MySlots) //run through all slots of a bag
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == type) //if a slot contains the item im looking for
                {
                    foreach (Item item in slot.MyItems)
                    {
                        items.Push(item);   //add item in the stack
                        if (items.Count == count) //if i reach the count i need then i dont need to look the rest of the inventory
                        {
                            return items;
                        }
                    }
                }
            }
        }
        return items;
    }

    public void OnItemCountChanged(Item item) //made it into a function so i can check if null to avoid NullRefExc
    {
        if (itemCountChangedEvent != null) //if sth is listening this
        {
            itemCountChangedEvent.Invoke(item);
        }
    }

    public List<SlotScr> GetAllItems() //get all slots with items, for saving
    {
        List<SlotScr> slots = new List<SlotScr>();
        foreach (Bag bag in MyBags)
        {
            foreach (SlotScr slot in bag.MyBagScr.MySlots)
            {
                if (!slot.IsEmpty)
                {
                    slots.Add(slot); //look through all inventory, every time i find a slot with an item on it, add the slot to the list and return the list at the end
                }
            }
        }
        return slots;
    }


    //for loading game TEST
    public void PlaceInCorrectPosition(Item item, int slotIndex, int bagIndex)
    {
        bags[bagIndex].MyBagScr.MySlots[slotIndex].AddItem(item);//first find the right bag, then the right slot and then place the item there
    }


    public void RemoveItem(Item item) //removes 1 of a specific item
    {
        foreach (Bag bag in MyBags) 
        {
            foreach (SlotScr slot in bag.MyBagScr.MySlots)
            {
                if (!slot.IsEmpty && slot.MyItem.MyTitle == item.MyTitle) 
                {
                    slot.RemoveItem(item);
                    break;
                }
            }
        }
    }
}
