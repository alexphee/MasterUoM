using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VendorItem
{
    [SerializeField]
    private Item item; //the vendor needs a ref to an actuall item
    [SerializeField]
    private int quantity;
    [SerializeField]
    private bool unlimited;

    public Item MyItem { get => item;}
    public int MyQuantity { get => quantity; set => quantity = value; }
    public bool Unlimited { get => unlimited; }
}
