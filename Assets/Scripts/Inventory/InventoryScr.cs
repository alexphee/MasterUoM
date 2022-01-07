using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScr : MonoBehaviour
{
    private static InventoryScr instance;
    public static InventoryScr MyInstance
    {
        //LOGIC: When i access the UIManager through MyInstance we check if the instance is set and if is not i set it and return it
        //Because of the structure i only have one instance of the game, i always return the same instance
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryScr>();
            }

            return instance;
        }
    }

    [SerializeField]
    public Item[] items; //DEBBUGING PURPOSES
    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]); //create bag -- casting, bad practice
        bag.Initialize(8); //initialize bag
        bag.Use(); //use bag
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
