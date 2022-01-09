using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMoveable
{
    [SerializeField]
    private Sprite icon; //the icon that is going to shown on the slot
    [SerializeField]
    private int stackSize; //some items are going to stack, this integer is going to define how many items can stack on themselves, it can also be set to zero and the item will not be stackable
    //when i pickup an item the player must be able to look at it and see what he is carrying, so i need properties with getters
    private SlotScr slot;
    public Sprite MyIcon { get => icon; } //no need for setters here, this will not change
    public int MyStackSize { get => stackSize; } //no need for setters here, this will not change

    //need ref to the slot the item is on. An item only exists in the game if it is placed in the inventory - if i drag it outside the inv on the ground i want it to get destroyed
    
    public SlotScr MySlot ///////////////IS THIS CORRECT ???
    {
        get { return slot; }
        set { slot = value; } //this needs a setter, i need to be able to move items from slot to slot
    }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}
