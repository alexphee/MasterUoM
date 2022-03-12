using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Element", menuName = "Items/element", order = 2)]

public class Element : Item
    {
    [SerializeField]
    private string symbol;

    [SerializeField]
    private string info;
    [SerializeField]
    private string info2;
    [SerializeField]
    private string phase;


    public override string GetDescription()
    {
        string info = string.Format("\nChemical Symbol: {0}\n {1}\n {2}\n {3}", symbol, this.info, this.info2, phase);
        return base.GetDescription() + info;
    }
}

