using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    //SINGLETON
    private static QuestGiverWindow instance;
    public static QuestGiverWindow MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestGiverWindow>();
            }
            return instance;
        }
    }
    private QuestGiver questGiver;

    [SerializeField]
    private GameObject questPrefab;
    [SerializeField]
    private Transform questArea;//the parent

    [SerializeField]
    private GameObject backBtn, acceptBtn, questDescription, completeBtn;

    private Quest selectedQuest;

    private List<GameObject> quests = new List<GameObject>();
    public void ShowQuests(QuestGiver questGiver)
    {
        this.questGiver = questGiver;
        foreach (GameObject gameObject in quests)
        {
            Destroy(gameObject);
        }
        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);


        foreach (Quest quest in questGiver.MyQuests) //run through every quest of this questgiver
        {
            if (quest != null) //this was added later // i need this because after i remove a quest from the list (after pressing Complete), i get a NullRefExc due to the implementation: questGiver.MyQuests[i] = null;
            {
                GameObject go = Instantiate(questPrefab, questArea);//instantiate the quest
                go.GetComponent<Text>().text = quest.MyTitle; //set the title

                go.GetComponent<QGQuestScr>().MyQuest = quest; //assign this quest to QGQScr  so i can use this quest to show sth in the Questgiver's window

                quests.Add(go); //everytime i add a quest to the questgiver i add the quest on this list so i keep track of what i have

                if (QuestLog.MyInstance.AlreadyHaveTheQuest(quest) && quest.IsComplete) //if i have the quest and i completed it then add OK at the end of the title
                {
                    go.GetComponent<Text>().text += "<size=8><color=green><i> Done</i></color></size>";
                }
                else if (QuestLog.MyInstance.AlreadyHaveTheQuest(quest))//if i already have the quest
                {
                    Color color = go.GetComponent<Text>().color; //take the color
                    color.a = 0.5f;
                    go.GetComponent<Text>().color = color; //turn quest gray-ish so i know i cant take it again
                }
            }
           
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest) //this will show info for a specific quest i click on // it will be called whenever i click on a quest, so many of the functionality will be here
    {
        this.selectedQuest = quest; //ref to the quest im viewing

        if (QuestLog.MyInstance.AlreadyHaveTheQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!QuestLog.MyInstance.AlreadyHaveTheQuest(quest))
        {
            acceptBtn.SetActive(true);
        }
       
        backBtn.SetActive(true);
        //acceptBtn.SetActive(true);
        questArea.gameObject.SetActive(false); //hide quests when click that
        questDescription.SetActive(true);

        //copy pasted this as is from questlog
        string objectives = "\n\n<i>Objectives</i>\n";
        string title = quest.MyTitle;
        string description = quest.MyDescription;
        foreach (Objective obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }
        questDescription.GetComponent<Text>().text = string.Format("<i>{0}</i>\n<size=8>{1}</size>", title, quest.MyDescription);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);
        ShowQuests(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        QuestLog.MyInstance.AcceptQuest(selectedQuest);
        Back(); //so i go back as soon as i accept the q
    }

    public override void Close() //QuestGiverWindow is a child of Window, Window has a close function already
    {
        completeBtn.SetActive(false); //if i close i make sure the complete button doesn't show on top of the other buttons
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])//if i reach a quest that is the same as the one i selected i need to remove it
                {
                    questGiver.MyQuests[i] = null; //apparently i cant use .Remove() on an array // this is the next best thing i guess
                }
            }
        }
        Back();//go back to first page
    }
}
