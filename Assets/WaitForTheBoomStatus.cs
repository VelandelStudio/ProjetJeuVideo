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
        Collider[] cols = Physics.OverlapSphere(explosionPos,radius);
        int nbMonsterTouched = GetComponentInChildren<DeflagrationSpell>().TargetsTouched.Count; // we count the number of monsters with the status touch
        // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it.
        /* for each collider (capsule collider only) triggered, damages and TouchStatus are applied */
        foreach (Collider col in cols)
        {
            if (!col.isTrigger) // if the target triggered is an Enemy monster and the collider is not a trigger apply damages and status
            {
                col.gameObject.GetComponent<EntityLivingBase>().DamageFor(((int)(Damages[0]*Mathf.Exp(nbMonsterTouched)))); // damages value is depending of the exponential fonction of the number of monsters with the touch status
                Debug.Log("Damages applied to Enemy Monster, PNJ and Player");
                Rigidbody rb = col.GetComponent<Rigidbody>();
                // apply damages to triggered targets Monster Player and pet
                //col.gameObject.GetComponent<CharacterController>().DamageFor(Damages[0]); // apply damages to triggered targets
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
                Debug.Log("EXPLOSION FORCE");
            }
        }
        Destroy(GetComponent<SummonerInterface>().Pet);
        Debug.Log("PET Destroy");
    }
}
