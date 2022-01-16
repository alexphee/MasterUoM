using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private VendorItem[] items;
    [SerializeField]
    private VendorWindow vendorWindow;

    public bool IsOpen { get; set; }
    public void Interact()
    {
        if (!IsOpen)
        {
            //Debug.Log("Interact with vendor");
            IsOpen = true;
           vendorWindow.CreatePages(items);
           vendorWindow.Open(this);
        }
    }

    public void StopInteraction()
    {
        if (IsOpen)
        {
            //Debug.Log("End interaction with vendor");
            IsOpen = false;
            vendorWindow.Close();
        }
       
    }
}
