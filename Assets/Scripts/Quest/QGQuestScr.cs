using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QGQuestScr : MonoBehaviour
{
    
    public Quest MyQuest { get; set; } //the questgiver questscr needs a ref to the actual quest

    public void Select()
    {
        //this will show the quest info
        QuestGiverWindow.MyInstance.ShowQuestInfo(MyQuest);
    }
}
