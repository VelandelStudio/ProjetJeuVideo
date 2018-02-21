using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonedStatus : StatusBase, IDebuff
{
    private EntityLivingBase _entity;
    private EnemyMonster _scriptParent;
    private float _timer;
    
    protected override void Start ()
    {

        base.Start();

        Debug.Log("HarpoonedStatus Created !");
        _scriptParent = GetComponentInParent<EnemyMonster>();
        _scriptParent.enabled = false;

        _timer = 0;
    }

    public void Update()
    {
        if (_timer == Duration)
        {
            Debug.Log("Timer : " + _timer + ", Duration : " + Duration);
            _scriptParent.enabled = true;
        }

        Mathf.Clamp(_timer + Time.deltaTime, 0, Duration);
    }

    public override void OnStatusApplied()
    {
        Debug.Log("HarpoonedStatus Created !");
        
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
