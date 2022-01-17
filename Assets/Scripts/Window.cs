using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private NPC npc;//this window is going to be handled by an NPC (vendor, questgiver etc)
    public virtual void Open(NPC npc)
    {
        this.npc = npc; //so that the window knows what vendor is using to show the items //without this i cant reopen the vendorwindow if i press X, i have to walk outside range and step back in
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        npc.IsInteracting = false;
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        npc = null;
    }
}
