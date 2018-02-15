using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderstormSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //apply damages on one target depending of the player stacks 
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
