using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackSummonerSupport : AutoAttackBase
{

    private GameObject _throwable;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;
    public List<SlowStatus> TargetsSlowed = new List<SlowStatus>(); //List of all targets with a TouchStatus.

    protected override void Start ()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>(); // instatiate player camera in a variable
        _throwable = LoadResource("AutoAttackSummonerSupport"); //  // instatiate player the resource to throw
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform); // instatiate player the Righthand player position from which the throwable is cast 

        base.Start();
    }

    protected override void Update()
    {
        TargetsSlowed.RemoveAll(SlowStatus => SlowStatus == null); // Remove all null TouchStatus.
        base.Update();
    }

    public override void AutoAttack()
    {
        if (AutoAttackIsReady())
        {
            base.AutoAttack();
            Instantiate(_throwable, _launcherTransform.position + _cameraPlayer.transform.forward * 2, _launcherTransform.rotation, this.transform); // instanciate the throwable object
        }
    }

    public void OnAttackHit(EntityLivingBase eHit)
    {
        eHit.DamageFor(Damages[0]); // apply damages to target
        Debug.Log("damages to target");

        /* Apply SlowStatus to  target */
        SlowStatus slow = eHit.gameObject.GetComponentInChildren<SlowStatus>();
        if (slow != null) // if already with SlowStatus => reset the status else apply the status
        {
            slow.ResetStatus();
            Debug.Log("Status reset on target");
        }
        else
        {
            GameObject obj = ApplyStatus(GetComponent<AutoAttackSummonerSupport>().Status[0], eHit.transform); // applying status to the Enemy Monster touched by the spell
            TargetsSlowed.Add(obj.GetComponent<SlowStatus>()); // update the list of EnemyMonster with "TouchStatus"
        }
    }

    protected virtual GameObject ApplyStatus(GameObject status, Transform tr)
    {
        GameObject objInst = Instantiate(status, tr);
        StatusBase statusInst = objInst.GetComponent<StatusBase>();
        statusInst.StartStatus(status.GetComponent<StatusBase>());
        return objInst;
    }
}
