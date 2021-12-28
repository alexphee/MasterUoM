using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);
public delegate void NPCRemoved();
public class NPC : Character
{
    public event HealthChanged healthChanged;
    public event NPCRemoved npcRemoved;
    [SerializeField]
    private Sprite portraitFace;
    public Sprite ThePortraitFace { get{ return portraitFace; } }
   public virtual void Deselect()
    {
        healthChanged -= new HealthChanged(UIManager.MyInstance.UpdateTargetFrame);
        npcRemoved -= new NPCRemoved(UIManager.MyInstance.HideTargetframe);
    }

    public virtual Transform Select()
    {
        return hitBox;
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null) //good practice in events. Always check if something is actually "listening", else i get NullRef exception
        {
            healthChanged(health);
        }
    }

    public void OnNPCRemoved()
    {
        if (npcRemoved != null) //maybe this is better, instead of if statement:  npcRemoved?.Invoke();
        {
            npcRemoved();
        }
        Destroy(gameObject);
    }
}
