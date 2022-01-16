using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text price;
    [SerializeField]
    private Text quantity;


    private VendorItem vendorItem;
    //i will make a function to set everything up
    public void AddItem(VendorItem vendorItem)
    {
        this.vendorItem = vendorItem;
        if (vendorItem.MyQuantity > 0 || (vendorItem.MyQuantity == 0 && vendorItem.Unlimited)) //no need to show sth if there is no quantity
        {
                icon.sprite = vendorItem.MyItem.MyIcon;
                title.text = string.Format("<color={0}>{1}</color>", TypeColor.MyTypeColors[vendorItem.MyItem.MyType], vendorItem.MyItem.MyTitle); //set the correct color for every item
                
                gameObject.SetActive(true);
                if (!vendorItem.Unlimited)//if it is not unlimited then set the quantity
                {
                    quantity.text = vendorItem.MyQuantity.ToString();
                }
            else
            {
                quantity.text = string.Empty; //if there is an item on the next page on the corresponding slot and is unlimited then dont show text // this was a bug
            }
            if (vendorItem.MyItem.MyPrice > 0)
            {
                price.text = "Price: " + vendorItem.MyItem.MyPrice.ToString();
            }
            else
            {
                price.text = string.Empty;
            }
            gameObject.SetActive(true);
        }
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Player.MyInstance.MyGold >= vendorItem.MyItem.MyPrice && InventoryScr.MyInstance.AddItem(vendorItem.MyItem)) //if i have enough gold and if i can add the item to the inventory
        {
            SellItem();
        }
    }

    private void SellItem()
    {
        Player.MyInstance.MyGold -= vendorItem.MyItem.MyPrice; //reduce gold by price
        if (!vendorItem.Unlimited) //if its not an unlimited item
        {
            vendorItem.MyQuantity--;
            quantity.text = vendorItem.MyQuantity.ToString();
            if (vendorItem.MyQuantity == 0)
            {
                gameObject.SetActive(false);//if no more items, hide
            }
        }
    }
}
