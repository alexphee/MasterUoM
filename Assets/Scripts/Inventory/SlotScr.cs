using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScr : MonoBehaviour, IPointerClickHandler, IClickable
{
    private ObservableStack<Item> items = new ObservableStack<Item>();

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;
    public bool IsEmpty
    {
        get { return items.Count == 0; } //no items in, empty slot
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
                return items.Peek();
            }
            return null;
        }
    }

    public Image MyIcon { get => icon; set => icon = value; }

    public int MyCount
    {
        get
        {
            return items.Count;
        }
    }

    public Text MyStackText
    {
        get { return stackSize; }
    }

    private void Awake()
    {
        items.OnPop += new UpdateStackEvent(UpdateSlot); //everytime i remove sth, the function will listen to that and called
        items.OnPush += new UpdateStackEvent(UpdateSlot);
        items.OnClear += new UpdateStackEvent(UpdateSlot); 
    }

    public bool AddItem(Item item)
    {
        items.Push(item);
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
            items.Pop(); //if used, remove
            //UIManager.MyInstance.UpdateStackSize(this);
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
            else if (InventoryScr.MyInstance != null)//if i hold sth to move
            {
                if (PutItemBack() || SwapItems(InventoryScr.MyInstance.FromSlot) || AddItems(InventoryScr.MyInstance.FromSlot.items)) //order is important
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
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);
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
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items); //make a copy of all the items i need to swap from slotA
            from.items.Clear(); //clear slotA so there is room there
            from.AddItems(items); //take all items from other slotB and copy to slotA
            items.Clear(); //Clear slotB
            AddItems(tmpFrom); //move items from copy A to B
            return true;
        }return false;
    }
    private void UpdateSlot() //gonna called everytime something changes items (Stack<Items> items)
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }
}