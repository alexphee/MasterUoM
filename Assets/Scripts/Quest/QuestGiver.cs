using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
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
        temp.AcceptQuest(quests[1]);
    }

}