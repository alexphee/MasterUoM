using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private VendorItem[] items;
    [SerializeField]
    private VendorWindow vendorWindow;
    public void Interact()
    {
        //Debug.Log("Interact with vendor");
        vendorWindow.CreatePages(items);
        vendorWindow.Open();
    }

    public void StopInteraction()
    {
        //Debug.Log("End interaction with vendor");
        vendorWindow.Close();
    }
}
