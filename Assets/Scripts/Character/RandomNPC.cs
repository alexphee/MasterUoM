using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNPC : NPC
{
    [SerializeField]
    private Dialogue dialogue;

    public override void Interact()
    {
        base.Interact();
        DialogueWindow.MyInstance.SetDialogue(dialogue); //when i RClick this npc, i will take the dialogue the npc has in editor and show it
    }
}
