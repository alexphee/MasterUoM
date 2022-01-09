using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        icon.transform.position = Input.mousePosition+offset;
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
}
