using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScr : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab; //the bag has to create slots so it needs a slot prefab. Serialize it so it accesses a prefab and instantiate it

    private CanvasGroup canvasGroup;//the use of canvasGroup is to hide and show UI elements, so i implement it heree

    private List<SlotScr> slots = new List<SlotScr>();//check for empty slots


    public bool IsOpen { get { return canvasGroup.alpha > 0; } }//if canvasgroup alpha is >0 then return true because its open else if it is zero its closed so return false

    public List<SlotScr> MySlots { get => slots; }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void AddSlots(int slotCount) //the amount of slots the bag script is going to create
    {
        for (int i = 0; i < slotCount; i++) 
        {
            SlotScr slot = Instantiate(slotPrefab, transform).GetComponent<SlotScr>(); //here i instantiate the slot prefab and with transform im putting it as child object of bagscript so the slots will be childs of the bag
            MySlots.Add(slot);
        }
    }

    public bool AddItem(Item item)
    {
        foreach (SlotScr slot in MySlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }return false;
    }
    public void OpenClose()
    {
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1; //alternate between 0 and 1 alpha, so its hidden/shown
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true; //open close inv bags
    }
}
