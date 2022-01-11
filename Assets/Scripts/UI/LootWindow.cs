using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootWindow : MonoBehaviour
{
    [SerializeField]
    private LootButton[] lootButtons;

    ////THIS IS FOR DEBUG PURPOSE
    [SerializeField]
    private Item[] items;
    // Start is called before the first frame update
    void Start()
    {
        AddLoot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddLoot()
    {
        string title = string.Format("<color={0}>{1}</color>", TypeColor.MyTypeColors[items[0].MyType], items[0].Mytitle);
        lootButtons[0].MyIcon.sprite = items[0].MyIcon; //take loot button and set the icon as the items icon
        lootButtons[0].gameObject.SetActive(true);    //make sure loot button is visible
        lootButtons[0].MyTitle.text = title;    //generate the title with right color

        lootButtons[0].MyLoot = items[0];
    }
}
