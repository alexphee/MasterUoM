using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagButton : MonoBehaviour, IPointerClickHandler
{
    private Bag bag;
    [SerializeField]
    private Sprite full, empty;

    public Bag MyBag
    {
        get { return bag; }
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = full;
            }
            else
            {
                GetComponent<Image>().sprite = empty;
            }
            bag = value;

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //if i click left button
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                 HandScr.MyInstance.TakeMoveable(MyBag); //if i hold shift and click, take the bag equipped to the button and hold it in the hand
            }
            else if(bag != null) //if i dont hold shift while clicking just open close
            {
            bag.MyBagScr.OpenClose(); //open-close bag
            }
        }
        
    }

    public void RemoveBag()
    {
        InventoryScr.MyInstance.RemoveBag(MyBag); //remove the bag attached to this button
        MyBag.MyBagButton = null;
        foreach (Item item in MyBag.MyBagScr.GetItems()) //take all items of the bag
        {
            InventoryScr.MyInstance.AddItem(item); //put them back to inv, after the dequipping
        }
        
        MyBag = null; //make sure this bag button doesnt have a ref to a bag anymore so i cant click the bagbutton to open-close////if i remove this line im not able to eqquip the next bag after deletion
    }
}

   

