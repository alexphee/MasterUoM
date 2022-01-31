using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Spell
{
    [SerializeField]
    private int manaCost;
    [SerializeField]
    private string name;
    [SerializeField]
    private int damage;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float castTime;
    [SerializeField]
    private GameObject spellPrefab;
    [SerializeField]
    private Color barColor; //the color of the casting bar when the spell is casted

    public string MyName { get => name; }
    public int MyDamage { get => damage; }
    public Sprite MyIcon { get => icon; }
    public float MySpeed { get => speed; }
    public float MyCastTime { get => castTime; }
    public GameObject MySpellPrefab { get => spellPrefab; }
    public Color MyBarColor { get => barColor; }
    public int MyManaCost { get => manaCost; set => manaCost = value; }
}
