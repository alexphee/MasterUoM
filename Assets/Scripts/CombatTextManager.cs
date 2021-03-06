using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum cType { DAMAGE, HEAL, XP, TEXT, MANA}
public class CombatTextManager : MonoBehaviour
{
    private static CombatTextManager instance;
    
    public static CombatTextManager MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = GameManager.FindObjectOfType<CombatTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject combatTextPrefab;
   
    public void CreateText(Vector2 position, string text, cType type)
    {
        Text t = Instantiate(combatTextPrefab, transform).GetComponent<Text>(); //text appears
        t.transform.position = position; //place txt in correct position

        string sign = string.Empty; //  +/-
        switch (type)
        {
            case cType.DAMAGE:
                sign = "-";
                t.color = Color.red;
                break;
            case cType.HEAL:
                sign = "+";
                    t.color = Color.green;
                break;
            case cType.XP:
                sign = "XP +";
                    t.color = Color.yellow;
                break; 
            case cType.TEXT:
                    t.color = Color.white;
                break;
            case cType.MANA:
                sign = "+";
                t.color = Color.blue;
                break;
        }
        t.text = sign + text;
    }
}
