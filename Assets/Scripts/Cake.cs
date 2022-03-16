using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Cake", menuName = "Items/Cake", order = 1)]

public class Cake : Item, IUseable
{
    public void Use()
    {
        Remove();
        SceneManager.LoadScene("Credits");
    }

    public override string GetDescription()
    {

        return base.GetDescription() + string.Format("\n This was a triumph! I'm making a note here: 'Huge success'");
    }
}
