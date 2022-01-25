using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Item item; //so i can see hat item needs to be set
    [SerializeField]
    private Image image;
    [SerializeField]
    private Text title;
    [SerializeField]
    private Text stack;
    private int count; //the count that will go in the stack

    public Item MyItem { get => item; set => item = value; }

    public void Initialize(Item item, int count) //takes in which item i need to create and how many of those
    {
        this.MyItem = item;
        this.image.sprite = item.MyIcon;
        this.title.text = string.Format("<color={0}>{1}</color>", TypeColor.MyTypeColors[item.MyType], item.MyTitle);
        this.count = count;
        if (count > 1) //show stack only if i have 2 or more
        {
            stack.enabled = true;
            stack.text = InventoryScr.MyInstance.GetItemCount(item.MyTitle).ToString() + "/" + count.ToString();
        }
    }

    public void UpdateStackCount()
    {
        stack.text = InventoryScr.MyInstance.GetItemCount(MyItem.MyTitle) + "/" + count.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //UIManager.MyInstance.ShowTooltip(new Vector2(0, 0), transform.posititon, MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //UIManager.MyInstance.HideTooltip();
    }
}
