using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScr : MonoBehaviour
{
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
    public bool CanAddBag { get { return bags.Count < 3; } }

    public int MyEmptySlotCount { //everytime i look at a bag i look at all slots and count them together and return them
        get
        {
            int count = 0;
            foreach (Bag bag in bags)
            {
                count += bag.MyBagScr.MyEmptySlotCount;
            }
            return count;
        }
    }
    public SlotScr FromSlot
    {
        get => fromSlot;
        set {
            fromSlot = value;
            if(value != null)
            {
                fromSlot.MyIcon.color = Color.gray; //if i click on any item in my inv this code is going to set the fromSlot equal to whatever i click on and thenm im graying it out
            }
        }
    }

    [SerializeField]
    private BagButton[] bagbuttons;


    [SerializeField]
    public Item[] items; //DEBBUGING PURPOSES
    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]); //create bag -- casting, bad practice
        bag.Initialize(8); //initialize bag
        bag.Use(); //use bag
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) {
            Bag bag = (Bag)Instantiate(items[0]); 
            bag.Initialize(8);
            bag.Use();
        }


        //////DEBUG
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(8);
            AddItem(bag);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
       if (Input.GetKeyDown(KeyCode.M))
        {
            Element carbon = (Element)Instantiate(items[2]);
            AddItem(carbon);
        }
       
    }

    public void AddBag(Bag bag) //takes the bag we are equiping
    {
        foreach (BagButton bagButton in bagbuttons)
        {
            if(bagButton.MyBag == null) //if i dont have a bag already
            {
                bagButton.MyBag = bag; //then assign a bag
                bags.Add(bag); //keep track of bag counter so it doesnt overfloat
                bag.MyBagButton = bagButton; //the bag itself has a ref to the button its sitting on, by creating that ref im able to use it whenever i remove the bag
                break; //dont do this for every bag, just one
            }
        }
    }

    public void RemoveBag(Bag bag)
    {
        bags.Remove(bag); //take the specific bag from bags list uptop 
        Destroy(bag.MyBagScr.gameObject);
    }

    public bool AddItem(Item item)
    {
        if(item.MyStackSize > 0)
        {
            if (PlaceInStack(item)){
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
        foreach (Bag bag in bags)
        {
            if (bag.MyBagScr.AddItem(item))
            {
                return true;
            }
        }return false;
    }
    private bool PlaceInStack(Item item)
    {
        foreach (Bag bag in bags) //check all bags
        {
            foreach (SlotScr slotScr in bag.MyBagScr.MySlots) //check all slots in bag
            {
                if (slotScr.StackItem(item)) //check if i can stack the item on some slot
                { 
                    return true;
                }
            }
        } return false;
    }

    public void OpenClose()
    {
        bool closedBag = bags.Find(x => !x.MyBagScr.IsOpen); //loop through all bags in list and if a single bag is closed then set closedBag to true. So if closedBag is true i know at least one bag is closed
        //if closed bag == true then open all closed bags
        //if closed bag == false then close all open bags
        foreach (Bag bag in bags)
        {
            if(bag.MyBagScr.IsOpen != closedBag)
            {
                bag.MyBagScr.OpenClose();
            }
        }
    }
}
