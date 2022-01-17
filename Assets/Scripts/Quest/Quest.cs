using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    [SerializeField]
    private string title;
    [SerializeField]
    private string description;

    [SerializeField]
    private CollectObjective[] collectObjectives; //array of collection quests
    public QuestScr MyQuestScr { get; set; }
    public string MyTitle { get => title; set => title = value; }
    public string MyDescription { get => description; set => description = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }

    public bool IsComplete
    {
        get
        {
            foreach (Objective objective in collectObjectives)
            {
                if (!objective.IsComplete) //if not completed
                {
                    return false;
                }
            }
            return true; //if nothing is NOT completed then true
        }
    }
}

[System.Serializable]
public abstract class Objective
{
    [SerializeField]
    private int amount;

    private int currentAmount;

    [SerializeField]
    private string type;

    public int MyAmount { get => amount; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string MyType { get => type; }

        public bool IsComplete {  //checks current amount compared to the goal amount
            get
            {
                return MyCurrentAmount >= MyAmount; //is the objective completed?
            }
        }
}

[System.Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item) 
    {
        //Debug.Log("UPDATEITEM CALLED");
        if (MyType.ToLower() == item.MyTitle.ToLower()) //if the item i picked up (fed to this function) is the same as the objective's item
        {
            MyCurrentAmount = InventoryScr.MyInstance.GetItemCount(item.MyTitle); 
            QuestLog.MyInstance.UpdateSelected();
            QuestLog.MyInstance.CheckCompletion(); //if itemcount is updated on a collection quest check completion
            Debug.Log("Current amount" + MyCurrentAmount);
        }
    }
}