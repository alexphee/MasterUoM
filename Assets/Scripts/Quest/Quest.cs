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
    [SerializeField]
    private KillObjective[] killObjectives; //array of kill quests
    public QuestScr MyQuestScr { get; set; }
    public QuestGiver MyQuestGiver { get; set; } //this ref is needed bc each quest has to know which questgiver it comes from so it can notify the questgiver when its completed
    public string MyTitle { get => title; set => title = value; }
    public string MyDescription { get => description; set => description = value; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
    public KillObjective[] MyKillObjectives { get => killObjectives; }
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
            foreach (Objective objective in killObjectives)
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
            if (MyCurrentAmount <= MyAmount) //this should stop messageFeed from showing progress after quest goal is completed //shows only till e.g. 3/3 and if i get 4/3 it doesnt show it
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount, MyAmount));
            }
            QuestLog.MyInstance.CheckCompletion(); //if itemcount is updated on a collection quest check completion
            QuestLog.MyInstance.UpdateSelected();
            //Debug.Log("Current amount" + MyCurrentAmount);
        }
    }

    public void UpdateItemCount() //overload UpdateItemCount //will try to update based on the items i have so i dont have to check the item first
    {
            MyCurrentAmount = InventoryScr.MyInstance.GetItemCount(MyType);
            QuestLog.MyInstance.CheckCompletion(); //if itemcount is updated on a collection quest check completion
            QuestLog.MyInstance.UpdateSelected();
    }

    public void Complete()
    {
        Stack<Item> items = InventoryScr.MyInstance.GetItems(MyType, MyAmount);

        foreach (Item item in items)
        {
            item.Remove();
        }
    }
}

[System.Serializable]
public class KillObjective : Objective
{
  public void UpdateKillCount(Character character) //self explanatory
    {
        if (MyType == character.type)
        {
            if(MyCurrentAmount < MyAmount) // this is to stop counting kills when a kill quest is completed
            {
                MyCurrentAmount++;
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyType, MyCurrentAmount, MyAmount));
                QuestLog.MyInstance.CheckCompletion();
                QuestLog.MyInstance.UpdateSelected();
            }
            
        }
    }

    
}