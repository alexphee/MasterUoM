using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }
    public InventoryData MyInventoryData { get; set; }
    public List<QuestData> MyQuestdata { get; set; }
    public List<QuestGiverData> MyQuestGiverData { get; set; }

    public DateTime MyDateTime { get; set; }
    public string MyScene { get; set; } //to save the scene im in
    public SaveData() //constructor to instantiate everything
    {
        MyInventoryData = new InventoryData();
        MyQuestdata = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
        MyDateTime = DateTime.Now; //when i hit save, save current timestamp
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public float MyXP { get; set; }
    public float MyMaxXP { get; set; }
    public float MyHealth { get; set; }
    public float MyMaxHealth { get; set; }
    public float MyMana { get; set; }
    public float MyMaxMana { get; set; }
    //i cant save a vector2 like these others, i have to break it into X and Y
    public float MyX { get; set; }
    public float MyY { get; set; }

    public PlayerData(int level, float xp, float maxXp, float health, float maxHealth, float mana, float maxMana, Vector2 position) //takes in the data i want to save
    {
        this.MyLevel = level;
        this.MyXP = xp;
        this.MyMaxXP = maxXp;
        this.MyHealth = health;
        this.MyMaxHealth = maxHealth;
        this.MyMana = mana;
        this.MyMaxMana = maxMana;
        this.MyY = position.y;
        this.MyX = position.x;

    }
}

[Serializable]
public class ItemData
{ //for saving inventory
    public string MyTitle { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }
    public int MyBagIndex { get; set; }
    public ItemData(string title, int stackCount = 0, int slotIndex = 0, int bagIndex = 0)
    {
        MyTitle = title;
        MyStackCount = stackCount;
        MySlotIndex = slotIndex;
        MyBagIndex = bagIndex;
    }
}

[Serializable]
public class InventoryData
{
    public List<BagData> MyBags { get; set; }
    public List<ItemData> MyItems { get; set; }
    public InventoryData()
    {
        MyBags = new List<BagData>();
        MyItems = new List<ItemData>();
    }
}
[Serializable]
public class BagData
{
    public int MySlotCount { get; set; }
    public int MyBagIndex { get; set; } //so it loads into the correct slot
    public BagData(int count, int index)
    {
        MySlotCount = count;
        MyBagIndex = index;
    }
}

[Serializable]
public class QuestData
{
    public string MyTitle { get; set; }
    public string MyDerscription { get; set; }
    public CollectObjective[] MyCollectObjectives { get; set; }
    public KillObjective[] MyKillObjectives { get; set; }
    public int MyQuestGiverID { get; set; }
    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverID)
    {
        MyTitle = title;
        MyDerscription = description;
        MyCollectObjectives = collectObjectives;
        MyKillObjectives = killObjectives;
        MyQuestGiverID = questGiverID;
    }
}
[Serializable]
public class QuestGiverData
{
    public List<string> MyCompletedQuests { get; set; }
    public int MyquestGiverID { get; set; } //so i know which giver is
    public QuestGiverData(int questGiverID, List<string> completedQuests)
    {
        this.MyquestGiverID = questGiverID;
        MyCompletedQuests = completedQuests;
    }
}

