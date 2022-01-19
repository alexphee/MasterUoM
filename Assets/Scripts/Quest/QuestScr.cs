using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScr : MonoBehaviour
{

    public Quest MyQuest { get; set; }
    private bool markedComplete = false;

    public void Select() //this going to be called whenever i click on a quest
    {
        GetComponent<Text>().color = Color.green;
        QuestLog.MyInstance.ShowDescription(MyQuest);
        Debug.Log("SELECT in QuestScr is OK");
    }

    public void Deselect()
    {
        GetComponent<Text>().color = Color.white;
    }

    public void IsComplete()
    {
        if (MyQuest.IsComplete && !markedComplete)
        {
            markedComplete = true;
            GetComponent<Text>().text += " <color=red><size=8>DONE</size></color>";
            MessageFeedManager.MyInstance.WriteMessage(string.Format("{0} (Completed)", MyQuest.MyTitle));
        }
        else if(!MyQuest.IsComplete)
        {
            markedComplete = false;
            GetComponent<Text>().text = MyQuest.MyTitle; //reset title to normal
        }
    }
}
