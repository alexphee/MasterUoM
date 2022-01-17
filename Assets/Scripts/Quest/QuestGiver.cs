using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField]
    private Quest[] quests; //array of all quests the NPC has

    //DEBUGGING ONLY
    [SerializeField]
    private QuestLog temp;
    private void Awake()
    {
        //Debbugging only
        temp.AcceptQuest(quests[0]);
    }

}
