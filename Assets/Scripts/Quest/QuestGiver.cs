using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests; //array of all quests the NPC has
    [SerializeField]
    private Sprite question, questionTemp, exclamation;
    public Quest[] MyQuests { get => quests; }

    private List<string> completedQuests = new List<string>();
    public List<string> MyCompletedQuests {
        get
        {
            return completedQuests;
        }
        set
        {
            completedQuests = value;
            foreach (string title in completedQuests) //run through every quest i have tha are already complete
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].MyTitle == title)//needs the check for null for the finel quest, it returns NullRefExc otherwise
                    {
                        quests[i] = null; //and remove quest
                    }
                }
            }
        }
    }
    [SerializeField]
    private SpriteRenderer statusRenderer;

    [SerializeField]
    private int questGiverID;//for loading
    public int MyQuestGiverID { get => questGiverID; } //for loading
    private void Start()
    {
        foreach (Quest quest in quests)
        {
            if (quest != null)
            {
                quest.MyQuestGiver = this; //each quest is going to be "assigned" to a questgiver, so when its over it notifies the questgiver
            }
        }
    }

    public void UpdateQuestStatus() //shows ! or ? based on the quest status
    {
        int count = 0;
        foreach (Quest quest in quests) //i need to check for every quest, if all are null then i dont have to show a symbol, else i need to check if completed or the player has it (hence 2 ? symbols)
        {
            if (quest != null)
            {
                if (quest.IsComplete && QuestLog.MyInstance.AlreadyHaveTheQuest(quest)) //check for completed quest and if the player has the quest
                {
                    statusRenderer.sprite = question; //then show a questionmark
                    break; //if at least one is complete then i dont have to search anymore
                }
                else if (!QuestLog.MyInstance.AlreadyHaveTheQuest(quest)) //if i dont have completed quests and the questgiver has a quest that the player hasn't
                {
                    statusRenderer.sprite = exclamation; //show exclamation
                    break;
                }
                else if (!quest.IsComplete && QuestLog.MyInstance.AlreadyHaveTheQuest(quest)) //if the quest isn't complete BUT i already have it 
                {
                    statusRenderer.sprite = questionTemp;
                }
            }
            else //this part of code will hide any symbol above questgiver after completing all quests
            {
                count++;
                if (count == quests.Length)
                {
                    statusRenderer.enabled = false;
                }
            }
        }
    }


    /*   //DEBUGGING ONLY
       [SerializeField]
       private QuestLog temp;
       private void Awake()
       {
           //Debbugging only
           temp.AcceptQuest(quests[0]);
       }*/

}
