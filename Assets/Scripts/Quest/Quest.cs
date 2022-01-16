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
    private CollectObj[] collectObjectives; //array of collection quests
    public QuestScr MyQuestScr { get; set; }
    public string MyTitle { get => title; set => title = value; }
    public string MyDescription { get => description; }
    public CollectObj[] MyCollectObjectives { get => collectObjectives; }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}

[System.Serializable]
public class CollectObj : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (MyType.ToLower() == item.MyTitle.ToLower())
        {
            MyCurrentAmount = InventoryScr.MyInstance.GetItemCount(item.MyTitle);
            QuestLog.MyInstance.UpdateSelected();
            Debug.Log("Current amount" + MyCurrentAmount);
        }
    }
}