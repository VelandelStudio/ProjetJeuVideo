using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackWindiator : AutoAttackBase {

    private float _WindiatorGCD = 2f;
    private Animator _anim;
    private WindiatorSimpleAttack _wsa;

    protected override void Start()
    {
        GCD = _WindiatorGCD;
        _anim = GetComponent<Animator>();
        _wsa = GetComponentInChildren<WindiatorSimpleAttack>();

        base.Start();
    }

    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            if (Input.GetMouseButtonDown(0))
            {
                _wsa.SwapEnableArmeCol();
                _anim.SetBool("AutoAttack", true);
                StartCoroutine(CoroutineOneSec());
            }

            base.AutoAttack();
        }
    }

    IEnumerator CoroutineOneSec()
    {
        yield return new WaitForSeconds(1f);

        if (_wsa.GetValueColArme())
        {
            _wsa.SwapEnableArmeCol();
        }

        _anim.SetBool("AutoAttack", false);
    }
}
