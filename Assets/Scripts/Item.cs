using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type { Element, Common}
public abstract class Item : ScriptableObject, IMoveable, IText
{
    [SerializeField]
    private Type type;
    public Type MyType { get => type; }

    [SerializeField]
    private string title;
    public string MyTitle { get => title; }


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

    

    [SerializeField]
    private int price;
    public int MyPrice { get => price; }

    public virtual string GetDescription() //return description of specific item //virtual bc i will need to override it
    {
       /* string color = string.Empty;
        switch (MyType)
        {
            case Type.Element:
                color = "#05d500";    ////moved to separate class TypeColor for reuseability
                break;
            case Type.Common:
                color = "#FFE6A5";
                break;
        }*/
        return string.Format("<color={0}>{1}</color>", TypeColor.MyTypeColors[type], MyTitle); //zero is going to be replaced with color and 1 with title
    }


    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }
}

