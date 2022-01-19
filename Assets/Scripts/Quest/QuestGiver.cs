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

    [SerializeField]
    private SpriteRenderer statusRenderer;

    private void Start()
    {
        foreach (Quest quest in quests)
        {
            quest.MyQuestGiver = this; //each quest is going to be "assigned" to a questgiver, so when its over it notifies the questgiver
        }
    }

    public void UpdateQuestStatus() //shows ! or ? based on the quest status
    {
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
                    statusRenderer.sprite = exclamation; //shoq exclamation
                    break;
                }
                else if (!quest.IsComplete && QuestLog.MyInstance.AlreadyHaveTheQuest(quest)) //if the quest isn't complete BUT i already have it 
                {
                    statusRenderer.sprite = questionTemp;
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
