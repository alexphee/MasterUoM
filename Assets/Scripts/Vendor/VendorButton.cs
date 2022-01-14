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

    //i will make a function to set everything up
    public void AddItem(VendorItem Vitem)
    {
        if (Vitem.MyQuantity > 0 || (Vitem.MyQuantity == 0 && Vitem.Unlimited)) //no need to show sth if there is no quantity
        {
                icon.sprite = Vitem.MyItem.MyIcon;
                title.text = string.Format("<color={0}>{1}</color>", TypeColor.MyTypeColors[Vitem.MyItem.MyType], Vitem.MyItem.MyTitle); //set the correct color for every item
                price.text = "Price: " + Vitem.MyItem.MyPrice.ToString();
                gameObject.SetActive(true);
                if (!Vitem.Unlimited)//if it is not unlimited then set the quantity
                {
                    quantity.text = Vitem.MyQuantity.ToString();
                }
        }
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
