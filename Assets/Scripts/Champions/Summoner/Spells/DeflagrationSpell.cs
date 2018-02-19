using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeflagrationSpell : Spell
{
    private Camera _cameraPlayer; // variable that will contain the player camera 
    public List<TouchStatus> TargetsTouched = new List<TouchStatus>(); //List of all targets with a TouchStatus.

    protected override void Start()
    {
        _cameraPlayer = this.GetComponentInChildren<Camera>(); // get player camera object
        base.Start();
    }

    protected override void Update()
    {
        TargetsTouched.RemoveAll(TouchStatus => TouchStatus == null); // Remove all null TouchStatus.
        base.Update();
    }

    public override void LaunchSpell()
    {
        if (IsSpellLauncheable())
        {
            RaycastHit hit; // raycast object for aiming the spell with the mouse
            /* if the raycast touch something launch the spell else return without cooldown */
            bool hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), _cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            if (hasFoundHitPoint)
            {
                base.LaunchSpell(); 
                Debug.Log("sort lancé");
            }
            else
            {
                return;
            }
           
            Collider[] cols = Physics.OverlapSphere(hit.point, 5f); // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it.

            /* for each EnemyMonster collider (capsule collider only) triggered, damages and TouchStatus are applied */
            foreach (Collider col in cols) 
            {
                if (col.gameObject.GetComponent<EnemyMonster>() && !col.isTrigger) // if the target triggered is an Enemy monster and the collider is not a trigger apply damages and status
                {
                    col.gameObject.GetComponent<EnemyMonster>().DamageFor(Damages[0]); // apply damages to triggered targets
                    Debug.Log("Damages applied to target");

                    /* Apply TouchStatus to triggered targets */
                    TouchStatus Touch = col.gameObject.GetComponentInChildren<TouchStatus>();
                    if (Touch != null) // if already with TouchStatus => reset the status else apply the status
                    {
                        Touch.ResetStatus();
                        Debug.Log("Status reset on target");
                    }
                    else
                    {
                        GameObject obj= ApplyStatus(GetComponent<DeflagrationSpell>().Status[0], col.transform); // applying status to the Enemy Monster touched by the spell
                        TargetsTouched.Add(obj.GetComponent<TouchStatus>()); // update the list of EnemyMonster with "TouchStatus"
                    }
                }
            }
            base.OnSpellLaunched(); // cooldown
        }
    }
}
