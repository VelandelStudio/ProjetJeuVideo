using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTheBoomStatus : StatusBase, IBuff {

    public override void OnStatusApplied()
    {
            Debug.Log("WaitForTheBoomStatus applied ! be careful the explosion is near !!!!");
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }


    private void OnDestroy()
    {
        float radius = float.Parse(OtherValues[0]);
        float power = 10.0F;   
        Vector3 explosionPos = GetComponent<SummonerInterface>().Pet.transform.position;
        Collider[] cols = Physics.OverlapSphere(explosionPos,radius); // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it.
       /* for each collider (capsule collider only) triggered, damages and TouchStatus are applied */
        foreach (Collider col in cols)
        {
            if (!col.isTrigger) // if the target triggered is an Enemy monster and the collider is not a trigger apply damages and status
            {

                col.gameObject.GetComponent<EntityLivingBase>().DamageFor(Damages[0]);
                Rigidbody rb = col.GetComponent<Rigidbody>();
                // apply damages to triggered targets Monster Player and pet
                //col.gameObject.GetComponent<CharacterController>().DamageFor(Damages[0]); // apply damages to triggered targets
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                Debug.Log("Damages applied to target");
            }
        }
        Destroy(GetComponent<SummonerInterface>().Pet);
        Debug.Log("Destroy");
    }




}
