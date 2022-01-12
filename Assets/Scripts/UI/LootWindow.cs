using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    [SerializeField]
    private LootButton[] lootButtons;

    private List<List<Item>> pages = new List<List<Item>>();
    ////THIS IS FOR DEBUG PURPOSE
    [SerializeField]
    private Item[] items;

    private int pageIndex = 0; //keep track the page number

    [SerializeField]
    private Text pageNumber; //ref to the page text, 1/1 etc
    [SerializeField]
    private GameObject nextButton, previousButton; //ref to the buttons

    // Start is called before the first frame update
    void Start()
    {
        List<Item> temp = new List<Item>();
        for (int i = 0; i < items.Length; i++)//debugging
        {
            temp.Add(items[i]);
        }
        CreatePages(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatePages(List<Item> items)
    {
        List<Item> page = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            page.Add(items[i]); //run through each item and add it to the current page
            if (page.Count == 4 || i == items.Count -1) //if current page is full // the OR is required bc it just stops at 4, eg. if i have 6 it doesnt add the last 2 and also doesnt add when items are less than 4
            {
                pages.Add(page); //add new page to the list
                page = new List<Item>(); //create the new page
            }
        }
        AddLoot();
    }

    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            
            pageNumber.text = pageIndex + 1 + "/" + pages.Count; //pageindex is zero indexed so i add 1
            previousButton.SetActive(pageIndex > 0);
            nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1); //the last part makes sure i dont have a next button at last page
            
            
            for (int i = 0; i < pages[pageIndex].Count; i++) //if 3 items droped page0 has 3 items, if 10 items droped page0 has 4 items
            {
                if (pages[pageIndex][i] != null) //to avoid NullRefExc if an item is looted already or deleted
                {
                    string title = string.Format("<color={0}>{1}</color>", TypeColor.MyTypeColors[pages[pageIndex][i].MyType], pages[pageIndex][i].Mytitle);
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyIcon; //take loot button and set the icon as the items icon
                    lootButtons[i].MyLoot = pages[pageIndex][i];
                    lootButtons[i].gameObject.SetActive(true);    //make sure loot button is visible
                    lootButtons[i].MyTitle.text = title;    //generate the title with right color
                }
                
            }
        }        
    }
    public void ClearButtons() //this function is necessary bc some items carried to other pages
    {
        foreach (LootButton button in lootButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1) //if the page index is not the last page // check if i have more pages to the right
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0) //check if im at first page // check if i have more pages to the left
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot) //takes the item im going to loot and remove it from loot window
    {
        pages[pageIndex].Remove(loot);

        if (pages[pageIndex].Count == 0) //if the page is empty
        {
            pages.Remove(pages[pageIndex]); //remove the specific empty page
            if (pageIndex == pages.Count && pageIndex > 0) //if im at the last page AND index is not zero //am i at the last page and do i have more pages before the page i removed?
            {
                pageIndex--; //go to previous pages
            }
            AddLoot(); //the loot needs to be recaclulated here so the lootwindow is updated after taking sth
        }
    }
}
