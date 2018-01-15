using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackDefault : AutoAttackBase
{

    protected override void Start()
    {
        base.Start();
    }

    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            base.AutoAttack();
            Debug.Log("Je lance l'autoattack par défaut !");
        }
    }
}
