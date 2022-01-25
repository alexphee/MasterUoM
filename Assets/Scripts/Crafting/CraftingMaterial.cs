using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftingMaterial 
{
    //
    [SerializeField]
    private Item item; //the item needed for crafting
    [SerializeField]
    private int count; //how many of those items needed

    public int MyCount { get => count; }
    public Item MyItem { get => item; }
}
