using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Window window;

    
    public bool IsInteracting { get; set; }

    public virtual void Interact()
    {

        if (!IsInteracting)
        {
            IsInteracting = true;
            window.Open(this); //if i interact with this NPC then open the window
        }
        //Debug.Log("LOOT");
    }

    public virtual void StopInteraction()
    {
        if (IsInteracting)
        {
            IsInteracting = false;
            window.Close();
        }
    }
}
