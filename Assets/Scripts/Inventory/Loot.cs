using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //so i can add it as subclass of another class and then see it in the inspector
public class Loot
{
    [SerializeField]
    private Item item; // when sth drops, the loot need to be releated with an item so it shows
    [SerializeField]
    private float dropChance;

    public Item MyItem { get => item; }
    public float MyDropChance { get => dropChance; }
}
