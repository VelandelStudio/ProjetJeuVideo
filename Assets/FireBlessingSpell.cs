using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlessingSpell : Spell
{

    private ConflagrationSpell Conflagration;
    private float DurationOfCritSuccess = 5.0f;
    private float tick;

    protected override void Start()
    {
        SpellCD = 30.0f;
        tick = 0;
        Conflagration = GetComponent<ConflagrationSpell>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!Conflagration.CritSuccess)
            return;

        tick += Time.deltaTime;
        if (tick >= DurationOfCritSuccess)
        {
            Debug.Log("Conflagration CritSuccess ended !");
            Conflagration.CritSuccess = false;
        }
    }

    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (!IsSpellLauncheable())
            return;

        tick = 0;
        Conflagration.CritSuccess = true;
        Debug.Log("Conflagration CritSuccess 100% for 5 sec !");
        base.OnSpellLaunched();
    }
}
