using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private List<QuestScr> questScripts = new List<QuestScr>(); //TEST

    private List<Quest> quests = new List<Quest>();
    [SerializeField]
    private GameObject questPrefab;
    [SerializeField]
    private Transform questParent; //need the parent so the actual quest goes under that, to be shown properly

    private Quest selected;

    [SerializeField]
    private Text questDescription;
   
    //SINGLETON 
    private static QuestLog instance;

    public static QuestLog MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestLog>(); //i only have 1 questlog so i can do this

            }
            return instance;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OpenClose();
        }
    }
    public void AcceptQuest(Quest quest) //the AcceptQuest asks the questgiver what quest am i accepting and the questgiver feeds this exact quest in this function
    {
        foreach (CollectObjective obj in quest.MyCollectObjectives)
        {
            InventoryScr.MyInstance.itemCountChangedEvent += new ItemCountChanged(obj.UpdateItemCount); //when i accept a quest i check all collectobjectives from that quest and io trigger this every time itemcount changes
            obj.UpdateItemCount(); //this had to be added later because when i already had 10 potions and THEN i clicked a quest that required 3 potions, it showed 0/10
        }
        quests.Add(quest); //add the quest i accepted on the list, so i check if i already have it or not
        GameObject gameObject = Instantiate(questPrefab, questParent); //instantiate the quest prefab from the folder into the gameworld
        QuestScr qs = gameObject.GetComponent<QuestScr>();//take the quest script from above gameobject and create ref
        quest.MyQuestScr = qs; //both ways ref
        qs.MyQuest = quest; //when i accept the quest from questgiver, i can take the quest and assign it to the questscript. The questscript has a ref to the original quest

        questScripts.Add(qs); //TEST


        gameObject.GetComponent<Text>().text = quest.MyTitle;
        CheckCompletion();
    
    }
    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest) //shows this specific quest's description
    {
        if (quest != null) //saves me from NullRefExc
        {
            if (selected != null && selected!=quest) //if sth is selected AND the one i have already selected is different from the new one im selecting and trying to show description from
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

    public void CheckCompletion()
    {
        Debug.Log("RUN FROM CHECK COMPLETION");
        foreach (QuestScr qs in questScripts)
        {
            Debug.Log("RUN FROM FOREACH");
            qs.IsComplete(); //checks if any quest is complete
        }
    }

    public void OpenClose()
    {
        if (canvasGroup.alpha == 1)
        {
            Close();
        }
        else
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    public void AbandonQuest()
    {
        //removes the quest from the quest log 
    }
    public bool AlreadyHaveTheQuest(Quest quest) //returns true if i already have a quest
    {
        Debug.Log("ALREADY HAVE QUEST");
        return quests.Exists(q => q.MyTitle == quest.MyTitle);
    }
}
