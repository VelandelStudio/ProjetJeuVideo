﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeSpell : Spell {

    public override void LaunchSpell()
    {
        // change current position of the player and pet then apply status "velocity" to them
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
        }
    }
}
