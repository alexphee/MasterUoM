using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="HealthPotion", menuName ="Items/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
    [SerializeField]
    private int health;
    public void Use()
    {
        if(Player.MyInstance.MyHealth.MyCurrentValue < Player.MyInstance.MyHealth.MyMaxValue)//if max health dont waste potion
        {
            Remove();
            //Player.MyInstance.MyHealth.MyCurrentValue += health; //REFACTORED
            Player.MyInstance.GetHealth(health); //use the function instead of the property
        }
        
    }

    public override string GetDescription()
    {
        // return base.GetDescription() + "\nUse: Restores 10 HP";
        return base.GetDescription() + string.Format("\nUse: Restores {0} health", health); //this is a better way to display health --easier to keep up with changes
    }
}
