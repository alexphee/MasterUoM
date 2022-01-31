using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="ManaPotion", menuName ="Items/manaPotion", order = 1)]
public class ManaPotion : Item, IUseable
{
    [SerializeField]
    private int mana;
    public void Use()
    {
        if(Player.MyInstance.MyMana.MyCurrentValue < Player.MyInstance.MyMana.MyMaxValue)
        {
            Remove();
            Player.MyInstance.GetMana(mana); 
        }
        
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nUse: Restores {0} mana", mana);
    }
}
