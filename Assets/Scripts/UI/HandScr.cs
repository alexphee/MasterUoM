using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScr : MonoBehaviour    //when im carruing sth i have an IMoveable. Im going to make a IMOveable property

{
    //singleton
    private static HandScr instance;
    public static HandScr MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<HandScr>();
            }return instance;
        }
    }
    public IMoveable MyMoveable { get; set; } //this is the moveable that this handscript is carrying around atm
    private Image icon; //the icon the player sees
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = Input.mousePosition + offset;

        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null)
        {
            DeleteItem();
        }
    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.MyIcon;
        icon.color = Color.white;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
    }

    private void DeleteItem()
    {
        if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && MyInstance.MyMoveable != null) //if i press the first mouse button and im not hovering over any UI elements and i have sth in my hand
        {
            if(MyMoveable is Item && InventoryScr.MyInstance.FromSlot != null) //checks if i carry sth that is an item with a ref to the inv
            {
                (MyMoveable as Item).MySlot.Clear();
            }
            Drop(); 
            InventoryScr.MyInstance.FromSlot = null; //i dont need a ref to that slot anymore. Resets fromslot
        }
    }
}
