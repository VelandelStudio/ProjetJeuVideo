using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackWindiator : AutoAttackBase {

    private float _WindiatorGCD = 2f;
    private Animator _anim;

    protected override void Start()
    {
        GCD = _WindiatorGCD;
        _anim = GetComponent<Animator>();

        base.Start();
    }

    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _anim.Play("AutoAttackWindiator",-1,0f);
            }

            base.AutoAttack();
        }
    }
}
