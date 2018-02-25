using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonedStunStatus : StatusBase, IDebuff
{
    private EntityLivingBase _entity;
    private EnemyMonster _scriptParent;
    private float _timer;

    protected override void Start()
    {

        base.Start();

        Debug.Log("HarpoonedStunStatus Created !");
        _scriptParent = GetComponentInParent<EnemyMonster>();
        _scriptParent.enabled = false;

        _timer = 0;
    }

    public void Update()
    {
        
        if (_timer == base.Duration)
        {
            _scriptParent.enabled = true;
        }

        _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, base.Duration);
    }

    public override void OnStatusApplied()
    {
        Debug.Log("HarpoonedStunStatus Created !");

    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
