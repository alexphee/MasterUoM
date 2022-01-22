using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScr : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    public int MyIndex { get; set; }//this is a test


    private ObservableStack<Item> items = new ObservableStack<Item>(); //this is a stack for every item in this slot

    [SerializeField]
    private Image icon; //ref to slot's icon

    [SerializeField]
    private Text stackSize;
    public BagScr MyBag { get; set; } //ref to the bag this slot belongs to

    public bool IsEmpty
    {
        get { return MyItems.Count == 0; } //no items in, empty slot
    }

    public bool IsFull//has it maxed out? or there is more room to fill on top of the stack
    {
        get
        { 
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }
            return true;
        }
    }
            
           
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }
            return null;
        }
    }

    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount
    {
        get
        {
            return MyItems.Count;
        }
    }

    public Text MyStackText
    {
        get { return stackSize; }
    }

    public ObservableStack<Item> MyItems { get => items; } //i had to encapsulate so i can access this from BagScript to get items

    private void Awake()
    {
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot); //everytime i remove sth, the function will listen to that and called
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot); 
    }

    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon; //take sprite on icon and assign it to the icon on the item im adding
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    public bool AddItems(ObservableStack<Item> newItems) //take item and place it in empty spot in inventory, also take item of same type ant stack it
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType()) { //check if slot is empty or the item on the slot has the same type
            int count = newItems.Count; //if i pop the loop doesnt run
            for (int i = 0; i < count; i++) //run through all items based on count
            {
                if (IsFull) 
                {
                    return false;
                }
                AddItem(newItems.Pop()); 
            }return true; //if i runned whole loop and add all items return true
        }return false; //else false
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            MyItems.Pop(); //if used, remove
            //UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void Clear()
    {
        int initCount = MyItems.Count;

        if(initCount > 0)
        {
            for (int i = 0; i < initCount; i++)
            {
                InventoryScr.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
            //MyItems.Clear();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)//on left click
        {
            if (InventoryScr.MyInstance.FromSlot == null && !IsEmpty)//if i dont hold anything to move AND the slot is non empty then i can pick up sth // empty slot throws NullRefExc
            {
                HandScr.MyInstance.TakeMoveable(MyItem as IMoveable); //the item sitting on the actual slot
                InventoryScr.MyInstance.FromSlot = this;
            }
            else if (InventoryScr.MyInstance.FromSlot == null && IsEmpty && (HandScr.MyInstance.MyMoveable is Bag)) //added later //if i click a bag holding shift im holding sth without setting fromslot to anything // if i click on this slot and the fromslot is null, this slot is empty and my handscr is carring a bag // normally when the fromslot is null the handscr wouldnt carry anything
            {
                Bag bag = (Bag)HandScr.MyInstance.MyMoveable; //casting bc of NullRefExc
                if (bag.MyBagScr != MyBag)//make sure i cant dequip bag inside itself
                {
                    if (InventoryScr.MyInstance.MyEmptySlotCount - bag.MySlotCount > 0) //make sure myemptyslotcount - the amount of slots there are in the bag im trying to dequip is greater than 0 then there is enough space to put bag away. //If i do >=0 one item is missing// Logic:The amount of slot left when dequipping the bag is greater than zero, then ok
                    {
                        AddItem(bag); //add item to inv
                        bag.MyBagButton.RemoveBag(); //remove from inv
                        HandScr.MyInstance.Drop(); //remove icon on hand
                    }
                    
                }
                
            }
            else if (InventoryScr.MyInstance != null)//if i hold sth to move
            {
                if (PutItemBack() || MergeItems(InventoryScr.MyInstance.FromSlot) || SwapItems(InventoryScr.MyInstance.FromSlot) || AddItems(InventoryScr.MyInstance.FromSlot.MyItems)) //order is important
                {
                    HandScr.MyInstance.Drop();
                    InventoryScr.MyInstance.FromSlot = null; //reset so i can do this again
                }
            }
            }
        if (eventData.button == PointerEventData.InputButton.Right) //on right click
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        if (InventoryScr.MyInstance.FromSlot == this) { //if this is true, im trying to put it back on the same slot //fromSlot==this
            InventoryScr.MyInstance.FromSlot.MyIcon.color = Color.white; //set color back to normal
            return true;
        } 
        return false;
    }

    private bool SwapItems(SlotScr from)
    {
        if (IsEmpty)
        {
            return false; //if empty no reasson to swap anything
        }
        if(from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize) //the first condition checks if the item i move is different than the item im clicking on, then swap. The second checks if fromSlots count plus the count on the slot im clicking on is larger than the total slot size of the items, swap.
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems); //make a copy of all the items i need to swap from slotA
            from.MyItems.Clear(); //clear slotA so there is room there
            from.AddItems(MyItems); //take all items from other slotB and copy to slotA
            MyItems.Clear(); //Clear slotB
            AddItems(tmpFrom); //move items from copy A to B
            return true;
        }return false;
    }

    public bool MergeItems(SlotScr from)
    {
        if (IsEmpty)
        {
            return false; //if empty, no reason for merge
        }
        if (from.MyItem.GetType() == MyItem.GetType()) //checks if the type of the item im holding is of the same type as the item im trying to merge
        {
            if (!IsFull) //check if slot is full
            {
                int free = MyItem.MyStackSize - MyCount; //calculate items left on slot to be full
                for (int i = 0; i < free; i++)
                {
                    AddItem(from.MyItems.Pop()); //add items to the slot by poping them from the holding item
                }
            }return true;
        }return false;
    }
    private void UpdateSlot() //gonna called everytime something changes items (Stack<Items> items)
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData) //show tooltip here
    {
        
        if (!IsEmpty)
        {
            // Debug.Log("Show tooltip");
            UIManager.MyInstance.ShowTooltip(transform.position, MyItem); //i can write MyItem since Item inherits from IText
        }
    }

    public void OnPointerExit(PointerEventData eventData) //hide tooltip here
    {
        // Debug.Log("Exit");
        UIManager.MyInstance.HideTooltip();

    }
}