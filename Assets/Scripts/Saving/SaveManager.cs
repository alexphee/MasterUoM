using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] //this need to be serialized so i can populate it from the inspector
    private Item[] items;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Game Saved !");
            Save();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Game Loaded!");
            Load();
        }
    }

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create); //creates the file in hard drive //THIS LINE DOESNT SAVE //there is also OpenOrCreate but it appends on old files

            SaveData data = new SaveData();
            SaveBags(data);
            SaveInventory(data);
            SavePlayer(data);
            SaveQuests(data);
            bf.Serialize(file, data); //this line actually saves the game by serializing the data
            file.Close();
        }
        catch (System.Exception)
        {

            throw;
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Player.MyInstance.MyLevel, Player.MyInstance.MyXP.MyCurrentValue, Player.MyInstance.MyXP.MyMaxValue,
                                            Player.MyInstance.MyHealth.MyCurrentValue, Player.MyInstance.MyHealth.MyMaxValue, Player.MyInstance.MyMana.MyCurrentValue,
                                            Player.MyInstance.MyMana.MyMaxValue, Player.MyInstance.transform.position);
    }

    private void SaveBags(SaveData data)
    {
        for (int i = 1; i < InventoryScr.MyInstance.MyBags.Count; i++) //start from 1 since i by default have at least 1 bag in inventory
        {
            data.MyInventoryData.MyBags.Add(new BagData(InventoryScr.MyInstance.MyBags[i].MySlotCount, InventoryScr.MyInstance.MyBags[i].MyBagButton.MyBagindex)); //check in the inventory the bag im looking atm, how many slots that bag has? i store that amount and the index
        }
    }

    private void SaveInventory(SaveData data)
    {
        List<SlotScr> slots = InventoryScr.MyInstance.GetAllItems();
        foreach (SlotScr slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyItems.Count, slot.MyIndex, slot.MyBag.MyBagIndex));
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach (Quest quest in QuestLog.MyInstance.MyQuests)
        {
            data.MyQuestdata.Add(new QuestData(quest.MyTitle, quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives, quest.MyQuestGiver.MyQuestGiverID)); //saving the quests
        }
    }
    private void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open); //finds teh saved file and opens it

            SaveData data = (SaveData)bf.Deserialize(file); //deserializes data and casts it as SaveData so i can load it
            
            file.Close();
            LoadBags(data);
            LoadInventory(data);
            LoadPlayer(data);
            LoadQuests(data);
        }
        catch (System.Exception)
        {

            //this is for handling errors
        }
    }

    private void LoadPlayer(SaveData data)
    {
        Player.MyInstance.MyLevel = data.MyPlayerData.MyLevel;
        Player.MyInstance.UpdateLevel();
        Player.MyInstance.MyHealth.Initialize(data.MyPlayerData.MyHealth, data.MyPlayerData.MyMaxHealth);
        Player.MyInstance.MyMana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Player.MyInstance.MyXP.Initialize(data.MyPlayerData.MyXP, data.MyPlayerData.MyMaxXP);
        Player.MyInstance.transform.position = new Vector2(data.MyPlayerData.MyX, data.MyPlayerData.MyY);
    }

    public void LoadBags(SaveData data)
    {
        foreach (BagData bagData in data.MyInventoryData.MyBags)
        {
            Bag newBag = (Bag)Instantiate(items[0]); //hierarchy-->SaveManager i know i have set the bag at 0
            newBag.Initialize(bagData.MySlotCount);
            InventoryScr.MyInstance.AddBag(newBag, bagData.MyBagIndex);
        }
    }

    private void LoadInventory(SaveData data)
    {
        foreach (ItemData itemData in data.MyInventoryData.MyItems) //look thorugh all items saved in savedata
        {
            Item item = Array.Find(items, x => x.MyTitle == itemData.MyTitle); //this will take the item
            for (int i = 0; i < itemData.MyStackCount; i++) //run thorugh all items i have
            {
                InventoryScr.MyInstance.PlaceInCorrectPosition(item, itemData.MySlotIndex, itemData.MyBagIndex);
            }
        }
    }

    private void LoadQuests(SaveData data)
    {
        QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>(); //find all questgivers
        foreach (QuestData questData in data.MyQuestdata)
        {
            QuestGiver qGiv = Array.Find(questGivers, x => x.MyQuestGiverID == questData.MyQuestGiverID); //run through all questgivers check ids, if the ids match any quest i heve then i store ref to questgiver
            Quest q = Array.Find(qGiv.MyQuests, x => x.MyTitle == questData.MyTitle);//run thorugh all quests inside questgivers and if i find sth with the same title as the questdata then make ref to that quest
            q.MyQuestGiver = qGiv; //the quest im going to add to the questlog needs ref toquestgiver so i know if its completed later on
            QuestLog.MyInstance.AcceptQuest(q);
        }
    }
}
