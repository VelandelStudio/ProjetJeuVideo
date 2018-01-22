using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSpell : Spell
{
    protected override void Start()
    {
        base.Start();
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (IsSpellLauncheable())
        {
            base.OnSpellLaunched();
            Debug.Log("Default spell launched");
        }
    }
}
