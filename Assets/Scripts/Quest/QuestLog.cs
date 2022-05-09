using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestLog : MonoBehaviour
{
    [SerializeField]
    private Text questCountTxt;
    [SerializeField]
    private int maxCount;
    private int currentCount;
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

    public List<Quest> MyQuests { get => quests; set => quests = value; }

    public void Start()
    {
        questCountTxt.text = currentCount + "/" + maxCount;
    }
    public void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Q))
         {
             OpenClose();
         }*/
    }
    public void AcceptQuest(Quest quest) //the AcceptQuest asks the questgiver what quest am i accepting and the questgiver feeds this exact quest in this function
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountTxt.text = currentCount + "/" + maxCount;

            foreach (CollectObjective objC in quest.MyCollectObjectives)
            {
                InventoryScr.MyInstance.itemCountChangedEvent += new ItemCountChanged(objC.UpdateItemCount); //when i accept a quest i check all collectobjectives from that quest and io trigger this every time itemcount changes
                objC.UpdateItemCount(); //this had to be added later because when i already had 10 potions and THEN i clicked a quest that required 3 potions, it showed 0/10
            }
            foreach (KillObjective objK in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmEvent += new KillConfirm(objK.UpdateKillCount);
            }
            MyQuests.Add(quest); //add the quest i accepted on the list, so i check if i already have it or not
            GameObject gameObject = Instantiate(questPrefab, questParent); //instantiate the quest prefab from the folder into the gameworld
            QuestScr qs = gameObject.GetComponent<QuestScr>();//take the quest script from above gameobject and create ref
            quest.MyQuestScr = qs; //both ways ref
            qs.MyQuest = quest; //when i accept the quest from questgiver, i can take the quest and assign it to the questscript. The questscript has a ref to the original quest

            questScripts.Add(qs); //TEST


            gameObject.GetComponent<Text>().text = quest.MyTitle;
            CheckCompletion();
        }


    }
    public void UpdateSelected()
    {
        ShowDescription(selected);
    }

    public void ShowDescription(Quest quest) //shows this specific quest's description
    {
        if (quest != null) //saves me from NullRefExc
        {
            if (selected != null && selected != quest) //if sth is selected AND the one i have already selected is different from the new one im selecting and trying to show description from
            {
                selected.MyQuestScr.Deselect(); //deselect
            }
            if (quest.MyGreekQuest)
            {
                string objectives = "\n\nΣτόχοι\n";
                selected = quest; //assign quest

                string title = quest.MyTitle;
                foreach (Objective obj in quest.MyCollectObjectives)
                {
                    objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
                }
                foreach (Objective obj in quest.MyKillObjectives)
                {
                    objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
                }
                questDescription.text = string.Format("<i>{0}</i>\n<size=8>{1}</size>{2}\n<color=yellow>Εμπειρία: {3} \nΧρυσός: {4} \n<color=orange>Ανταμοιβή: {5}</color></color>", title, quest.MyDescription, objectives, quest.MyExperience, quest.MyGoldReward, quest.MyRewardItem);
            } else
            {
                string objectives = "\n\nObjectives\n";
                selected = quest; //assign quest

                string title = quest.MyTitle;
                foreach (Objective obj in quest.MyCollectObjectives)
                {
                    objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
                }
                foreach (Objective obj in quest.MyKillObjectives)
                {
                    objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
                }
                questDescription.text = string.Format("<i>{0}</i>\n<size=8>{1}</size>{2}\n<color=yellow>Experience: {3} \nGold: {4} \n<color=orange>Reward: {5}</color></color>", title, quest.MyDescription, objectives, quest.MyExperience, quest.MyGoldReward, quest.MyRewardItem);
            }
            
            
            
            }
    }

    public void CheckCompletion()
    {
        //Debug.Log("RUN FROM CHECK COMPLETION");
        foreach (QuestScr qs in questScripts)
        {
            //Debug.Log("RUN FROM FOREACH");
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
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
    public void DropQuest() //unassign  the events
    {
        foreach (CollectObjective objC in selected.MyCollectObjectives)
        {
            InventoryScr.MyInstance.itemCountChangedEvent -= new ItemCountChanged(objC.UpdateItemCount); //unassign event
        }
        foreach (KillObjective objK in selected.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmEvent -= new KillConfirm(objK.UpdateKillCount); //unassign event
        }
        RemoveQuest(selected.MyQuestScr); //remove this from the questlog
    }

    public void RemoveQuest(QuestScr questScr) //takes the qusetscript i have in log and i need to remove it uppon completion
    {
        questScripts.Remove(questScr); //remove it from the list of quests
        Destroy(questScr.gameObject); //so i cant see it in the questlog
        MyQuests.Remove(questScr.MyQuest); //remove quest from the list of quests
        questDescription.text = string.Empty; //clear description of the quest
        selected = null; //deselecting the quest
        currentCount--; //minus one quest
        questCountTxt.text = currentCount + "/" + maxCount; //update the count
        questScr.MyQuest.MyQuestGiver.UpdateQuestStatus();
        questScr = null; //drop ref
    }
    public bool AlreadyHaveTheQuest(Quest quest) //returns true if i already have a quest
    {
        //Debug.Log("ALREADY HAVE QUEST");
        return MyQuests.Exists(q => q.MyTitle == quest.MyTitle);
    }
}