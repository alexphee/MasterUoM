using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestScr : MonoBehaviour
{

    public Quest MyQuest { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        if (MyQuest.IsComplete)
        {
            GetComponent<Text>().text += " (Completed)";
        }
    }
}
