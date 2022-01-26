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
    private Text count; //shows the amount of items i can craft based on materiasl

    private int maxAmount; //max amount of products i can create
    private int amount; //the amount of items i want to create

    [SerializeField]
    private ItemInfo craftItemInfo;

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
        foreach (GameObject material in materials)
        {
            ItemInfo ii = material.GetComponent<ItemInfo>();
            ii.UpdateStackCount();
        }
    }

    public void Craft()
    {
        StartCoroutine(CraftRoutine(0));
    }

    private IEnumerator CraftRoutine(int count)
    {
        yield return Player.MyInstance.MyInitRoutine = StartCoroutine(Player.MyInstance.CraftRoutine(selectedRecipe));
    }

    public void AddItemsToInventory() //ads the crafted item to inventory
    {
        InventoryScr.MyInstance.AddItem(craftItemInfo.MyItem);
    }
}
