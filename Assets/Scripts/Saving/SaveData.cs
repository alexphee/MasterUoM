using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }

    public SaveData() //constructor to instantiate everything
    {
        
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }

    public PlayerData(int level) //takes in the level i want to save
    {
        this.MyLevel = level;
    }
}
