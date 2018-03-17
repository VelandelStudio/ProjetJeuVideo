using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** HarpoonedStunStatus, public class
 * @extends StatusBase
 * This script allows to stun the monster hited.
 **/
public class HarpoonedStunStatus : StatusBase, IDebuff
{
    private EntityLivingBase _entity;
    private EnemyMonster _scriptParent;
   // private float _timer;



    public override void OnStatusApplied()
    {
        Debug.Log("HarpoonedStunStatus Created !");
        _scriptParent = GetComponentInParent<EnemyMonster>();
        _scriptParent.enabled = false;
    }

    /** Start, protected override void method
     * Disable the script of the parent gameObject.
     **/
    /*
    protected override void Start()
    {

        base.Start();

        Debug.Log("HarpoonedStunStatus Created !");
        _scriptParent = GetComponentInParent<EnemyMonster>();
        _scriptParent.enabled = false;

        _timer = 0;
    }
    */
    /** Update, protected override void method
     * Enables the script of the Monster when the timer equals to duration.
     **/
     /*
    public void Update()
    {
        
        if (_timer == base.Duration)
        {
            _scriptParent.enabled = true;
        }

        _timer = Mathf.Clamp(_timer + Time.deltaTime, 0, base.Duration);
    }
    */

    public override void DestroyStatus()
    {
        _scriptParent.enabled = true;

        base.DestroyStatus();
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
