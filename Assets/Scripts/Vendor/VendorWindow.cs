using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : Window
{
    
    [SerializeField]
    private VendorButton[] vendorButtons;

    List<List<VendorItem>> pages = new List<List<VendorItem>>(); //same as loot window

    private int pageIndex; //keep track of the page im on atm
    [SerializeField]
    private Text pageNumber;

    private Vendor vendor;
    public void CreatePages(VendorItem[] items) //i can take the items array of Vendor and show them
    {
        pages.Clear(); //clear pages b4 creating them
        List<VendorItem> page = new List<VendorItem>();
        for (int i = 0; i < items.Length; i++) //run through all items and create paegs based on that
        {
            page.Add(items[i]);
            if (page.Count == 10 || i == items.Length - 1) //if page is full or i dont have anymore items to fill
            {
                pages.Add(page);
                page = new List<VendorItem>(); //new list for next page
            }
        }
        AddItems();
    }
    public void AddItems()
    {
        pageNumber.text = pageIndex + 1 + "/" + pages.Count;
        if (pages.Count > 0)        
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    vendorButtons[i].AddItem(pages[pageIndex][i]); //access pages, access index of the pages, take the item and give it to the button
                }
            }
        }
    }
    

    public void NextPage()
    {
        if (pageIndex < pages.Count -1)
        {
            ClearButtons();
            pageIndex++;
            AddItems();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            ClearButtons();
            pageIndex--;
            AddItems();
        }
    }
    public void ClearButtons()
    {
        foreach (VendorButton vb in vendorButtons)
        {
            vb.gameObject.SetActive(false);
        }
    }

    public override void Open(NPC npc)
    {
        CreatePages((npc as Vendor).MyItems);
        base.Open(npc);
    }
}
