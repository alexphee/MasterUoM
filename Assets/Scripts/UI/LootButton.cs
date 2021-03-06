using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerClickHandler
{   
    //serialize the fields so i can set them in Unity inspector and take whatever they are referencing anmd set it inside loot window
    [SerializeField]
    private Image icon;
    [SerializeField]
    private Text title;

    private LootWindow lootWindow;
    public Image MyIcon { get => icon; }
    public Text MyTitle { get => title; }


    public Item MyLoot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryScr.MyInstance.AddItem(MyLoot)) //this statement return a bool
        {
            gameObject.SetActive(false); //disable game object
            lootWindow.TakeLoot(MyLoot); //take loot attached to the specific btn
        }
    }
}
