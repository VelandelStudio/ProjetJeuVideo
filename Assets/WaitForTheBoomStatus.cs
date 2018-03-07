using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTheBoomStatus : StatusBase, IBuff {

    public bool IsSpellUsable
    {
        get { return IsSpellUsable; }
        protected set { IsSpellUsable = false; }
    }


    public override void OnStatusApplied()
    {
        Debug.Log("WaitForTheBoomStatus applied ! be careful the explosion is near !!!!");
        //GetComponentInParent<NeutralFormSpell>().IsSpellUsable{get{ }; protected set {IsSpellUsable = false; } };
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        float radius = float.Parse(OtherValues[0]);
        float power = 100.0F;   
        Vector3 explosionPos = GetComponentInParent<SummonerInterface>().Pet.transform.position;
        Collider[] cols = Physics.OverlapSphere(explosionPos,radius);
        int nbMonsterTouched = GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count; // we count the number of monsters with the status touch
        // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it.
        

        //Rigidbody PlayerRigidBody = this.AddComponent<Rigidbody>(); // Add the rigidbody to the Player
       // PlayerRigidBody.mass = 1;

        /* for each collider  triggered, damages and EXPLOSION force are applied */
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Player" || col.gameObject.GetComponent<EntityLivingBase>() && !col.isTrigger)//&& !GetComponentInParent<SummonerInterface>().Pet) // if the target triggered is an Enemy monster and the collider is not a trigger apply damages and status
            {
                Debug.Log(col);
                if (col.gameObject.tag != "Player") // if the collider is own by an EntityLivingBase
                {
                    col.gameObject.GetComponent<EntityLivingBase>().DamageFor(((int)(Damages[0] * Mathf.Exp(nbMonsterTouched)))); // apply damages to triggered targets Monster Player and pet, damages value is depending of the exponential fonction of the number of monsters with the touch status
                    Debug.Log("Damages applied to Enemy Monster, PNJ and Player");
                }

                Rigidbody rb = col.GetComponent<Rigidbody>(); // instanciation of rigibody of the targets caught in the EXPLOSION
                if (rb != null)
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F); // projection force applied on the rigibodies caught in the EXPLOSION 
                Debug.Log("EXPLOSION FORCE");
            }
        }
        GetComponentInParent<DeflagrationSpell>().TargetsTouched.RemoveAll(TouchStatus => TouchStatus!=null); // remove all 
        //Destroy(this.GetComponent<Rigidbody>()); // destruction of the Player's rigidbody
        Destroy(GetComponentInParent<SummonerInterface>().Pet); // TOTAL DESTRUCTION OF THE PET !!!!!!
        Debug.Log("PET Destroy");
    }

    /*private T AddComponent<T>()
    {
        throw new NotImplementedException();
    }*/
}
