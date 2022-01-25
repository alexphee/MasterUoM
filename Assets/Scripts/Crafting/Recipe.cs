using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    [SerializeField]
    private CraftingMaterial[] materials; //this contains all the materials that are required for a recipe
    [SerializeField]
    private Item output; //the output item of the recipe
    [SerializeField]
    private int outputCount;

    [SerializeField]
    private string description; //recipe's description
    [SerializeField]
    private Image highlight;

    public Item MyOutput { get => output; }
    public int MyOutputCount { get => outputCount; set => outputCount = value; }
    public string MyDescription { get => description; }
    public CraftingMaterial[] MyMaterials { get => materials; }

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = output.MyTitle; //make sure i have the right name for the recipe
    }


   public void Select() //show highlight when clicking on recipe
    {
        Color color = highlight.color;
        color.a = 0.3f;
        highlight.color = color;
    }
    public void Deselect() //hide highlight
    {
        Color color = highlight.color;
        color.a = 0f;
        highlight.color = color;
    }
}
