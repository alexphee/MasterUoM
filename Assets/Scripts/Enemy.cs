using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    [SerializeField]
    private CanvasGroup healthGroup;

    public override Transform Select()
    {
        healthGroup.alpha = 1; //makes sure i can see the healthbar when i select the target

        return base.Select();
    }

    public override void Deselect()
    {
        healthGroup.alpha = 0; //hide healthbar
        base.Deselect();
    }
}
