using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crafting : MonoBehaviour //this is attached to recipe and will be responsible for showing the info when a recipe is clicked
{
    [SerializeField]
    private Text title;//title of the item to be produced
    [SerializeField]
    private Text description;
    [SerializeField]
    private GameObject materialPrefab; //to instantiate the materials
    [SerializeField]
    private Transform parent; //to put the materials to the correct parent, so they don't just appear random on the canvas

    private List<GameObject> materials = new List<GameObject>(); //!!!!! i need this to track all materials i have for a recipe bc i have to destroy them so when i click on different recipes materials dont stack forever

    [SerializeField]
    private Recipe selectedRecipe;
    [SerializeField]
    private Text counttxt; //shows the amount of items i can craft based on materiasl

    //private int maxAmount; //max amount of products i can create
   // private int amount; //the amount of items i want to create

    [SerializeField]
    private ItemInfo craftItemInfo;


    private List<int> amounts = new List<int>(); //for multi crafting


    [SerializeField]
    private CanvasGroup canvasGroup;
    private void Start()
    {
        InventoryScr.MyInstance.itemCountChangedEvent += new ItemCountChanged(UpdateMaterialCount);
        ShowDescription(selectedRecipe);
    }



    public void ShowDescription(Recipe recipe)
    {
        if (selectedRecipe != null)
        {
            selectedRecipe.Deselect(); //first deselect if sth is selected
        }
        this.selectedRecipe = recipe;
        this.selectedRecipe.Select();

        foreach (GameObject gameObject in materials) //BUGFIX when clicking recipe again it just adds materials to the description
        {
            Destroy(gameObject);
        }
        materials.Clear();

        title.text = recipe.MyOutput.MyTitle;
        description.text = recipe.MyDescription + " " + recipe.MyOutput.MyTitle.ToLower();

        craftItemInfo.Initialize(recipe.MyOutput, 1); //craft 1
        foreach (CraftingMaterial material in recipe.MyMaterials)
        {
            GameObject go = Instantiate(materialPrefab, parent);
            go.GetComponent<ItemInfo>().Initialize(material.MyItem, material.MyCount);
            materials.Add(go);
        }
        UpdateMaterialCount(null);
    }

    private void UpdateMaterialCount(Item item) //this will run everytime i get a new item in the inventory //there is no need for an item, i add it so the delegate will look for this function structure to trigger the event
    {
        amounts.Sort();
        foreach (GameObject material in materials)
        {
            ItemInfo ii = material.GetComponent<ItemInfo>();
            ii.UpdateStackCount();
        }
       /* if (CanCraft())
        {
            maxAmount = amounts[0];
            if (counttxt.text == "0")
            {
                counttxt.text = "1";
                amount = 1; //default
            }
            else if (int.Parse(counttxt.text) > maxAmount)
            {
                counttxt.text = maxAmount.ToString();
                amount = maxAmount;
            }
            else
            {
                counttxt.text = "0";
                amount = 0;
            }
        }*/
    }

    public void Craft()
    {

        if (CanCraft()) //only start the coroutine if there are enough materials for crafting
        {
            StartCoroutine(CraftRoutine());
        }
    }

    private bool CanCraft()//this will tell if i can craft sth or not
    {
        bool canCraft = true; //var to store if i can craft sth or not
        amounts = new List<int>();
        foreach (CraftingMaterial material in selectedRecipe.MyMaterials) //go through all materials
        {
            int count = InventoryScr.MyInstance.GetItemCount(material.MyItem.MyTitle);
            if (count >= material.MyCount)//run thorugh all materials and if at least 1 of the required ones is not enough then cancraft=false;
            {
                amounts.Add(count / material.MyCount); //a list containing the count for every material
                continue; //go to the next loop
            }
            else
            {
                canCraft = false;
                break;
            }
        }
        return canCraft;
    }
    private IEnumerator CraftRoutine()
    {
        yield return Player.MyInstance.MyInitRoutine = StartCoroutine(Player.MyInstance.CraftRoutine(selectedRecipe));
    }

    public void AddItemsToInventory() //ads the crafted item to inventory
    {
        if (InventoryScr.MyInstance.AddItem(craftItemInfo.MyItem))//if the item is successfuly added then remove material
        {
            foreach (CraftingMaterial material in selectedRecipe.MyMaterials) //for each material, remove it from inv
            {
                for (int i = 0; i < material.MyCount; i++)
                {
                    InventoryScr.MyInstance.RemoveItem(material.MyItem);
                }
            }
        }
        //InventoryScr.MyInstance.AddItem(craftItemInfo.MyItem);
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }
}