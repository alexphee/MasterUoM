using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestItem", menuName = "Items/QuestItem", order = 1)]

public class QuestItem : Item
{

    [SerializeField]
    private string descr;
    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\n"+descr);
    }
}
