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
        this.MyMaxHealth =maxHealth;
        this.MyMana = mana;
        this.MyMaxMana = maxMana;
        this.MyY = position.y;
        this.MyX = position.x;

    }
}
