using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackSummonerAOE : AutoAttackBase
{

    private GameObject _throwable;
    private Camera _cameraPlayer;
    private Transform _launcherTransform;

    protected override void Start ()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>(); // instatiate player camera in a variable
        _throwable = LoadResource("AutoAttackSummonerAOE"); //  // instatiate player the resource to throw
        _launcherTransform = PosHelper.GetRightHandTransformOfPlayer(transform); // instatiate player the Righthand player position from which the throwable is cast 

        base.Start();
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
        //if (eHit.GetComponent<EnemyMonster>()) // If you want to protect the EntityLivingBase GameObject other than EnemyMonster from the auto attack and status
       // {
            eHit.DamageFor(Damages[0]); // apply damages to the "primary target"
            Debug.Log("damages to target");

            Collider[] cols = Physics.OverlapSphere(eHit.transform.position, float.Parse(OtherValues[0])); // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it.
                                                                                                           /* apply half of the auto attackt damages to every monsters surrounding the target (2f range)  */
            foreach (Collider col in cols)
            {
                if (col.gameObject.GetComponent<EnemyMonster>() && !col.isTrigger && col.gameObject.GetComponent<EnemyMonster>() != eHit) // if the collider is not a trigger and is owned by an EnemyMonster and is not the primary target (already hit) 
                {
                    col.gameObject.GetComponent<EnemyMonster>().DamageFor(System.Convert.ToInt32(Damages[0] * 0.5)); // apply half of damages
                    Debug.Log("explosion damages !!!!!");
                }
            }
       // }
    }
}
