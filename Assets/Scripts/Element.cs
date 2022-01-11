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
    private int atomicNo;
    [SerializeField]
    private string phase;


    public override string GetDescription()
    {
        string info = string.Format("\nChemical Symbol: {0}\nAtomic Number: {1}\nPhase (in STP): {2}", symbol, atomicNo, phase);
        return base.GetDescription() + info;
    }
}

