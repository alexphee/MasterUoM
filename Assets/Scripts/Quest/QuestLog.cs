using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private GameObject questPrefab;
    [SerializeField]
    private Transform questParent;

    private Quest selected;

    [SerializeField]
    private Text questDescription;
    private static QuestLog instance;

    public static QuestLog MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<QuestLog>(); //i only have 1 questlog so i can do this

            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AcceptQuest(Quest quest)
    {
        foreach (CollectObj obj in quest.MyCollectObjectives)
        {
            
        }
        GameObject gameObject = Instantiate(questPrefab, questParent); //instantiate the quest prefab from the folder into the gameworld

        QuestScr questScr = gameObject.GetComponent<QuestScr>();//take the quest script from above gameobject and create ref
        questScr.MyQuest = quest; //when i accept the quest from questgiver, i can take the quest and assign it to the questscript. The questscript has a ref to the original quest
        quest.MyQuestScr = questScr; //both ways ref

        gameObject.GetComponent<Text>().text = quest.MyTitle;
    
    }

    public void ShowDescription(Quest quest) //shows a ques's description
    {
        if (selected != null) //if sth is selected
        {
            selected.MyQuestScr.Deselect(); //deselect
        }
        string objectives = "\n\nObjectives\n";
        selected = quest; //assign quest

        string title = quest.MyTitle;
        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }
        questDescription.text = string.Format("<i>{0}</i>\n<size=8>{1}</size>{2}", title, quest.MyDescription, objectives);
    }
}
