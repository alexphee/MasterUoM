using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer spriteRenderer; //ref to spriterenderer so i can change the sprite to open chest or closed chest
    [SerializeField]
    private Sprite openS, closeS; //the 2 sprites to be set
    
    private bool IsOpen; //i will use this to know if the chest is open or closed
    
    [SerializeField]
    private LootTable lootTable;

    public void Interact()
    {
        if (IsOpen)
        {
            LootWindow.MyInstance.Close();
            StopInteraction(); //if it is already open, stop interaction and close it
        }
        else
        {
            IsOpen = true; //if it is not open, i set isopen to true
            spriteRenderer.sprite = openS; //and i change the sprite to open

            lootTable.ShowLoot();

        }
    }

    public void StopInteraction()
    {
        IsOpen = false;
        spriteRenderer.sprite = closeS;

    }




}
