using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTheBoomStatus : StatusBase, IBuff {



    public override void OnStatusApplied()
    {
        
        Debug.Log("WaitForTheBoomStatus applied ! be careful the explosion is near !!!!");
        /* adding a rigibody to the Player to apply explosion force on him */
        //GameObject Player = transform.parent.gameObject; //Instanciation of the Player in a variable
        //Rigidbody PlayerRigidBody = Player.AddComponent<Rigidbody>(); // Add a rigidbody to the Player
        //PlayerRigidBody.mass = 1;

        GetComponentInParent<NeutralFormSpell>().IsSpellUsable = false;
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }

    private void OnDestroy()
    {
        if (GetComponentInParent<SummonerInterface>().Pet != null) //if the Pet is still alive then we apply damages and force to Everything with HP in the EXPLOSION area
        { 
            float radius = float.Parse(OtherValues[0]); // area of effect of the Explosion 
            float power = 100.0F;   // Explosion Force value
            int nbMonsterTouched = GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count; // we count the number of monsters with the status touch
            Debug.Log("exp("+nbMonsterTouched+") = "+Mathf.Exp(nbMonsterTouched));
            int ExplosionDamages = ((int)(Damages[0] * Mathf.Exp(nbMonsterTouched))); // Damages value 
            Vector3 explosionPos = GetComponentInParent<SummonerInterface>().Pet.transform.position; // center of the explosion is the Pet
            Collider[] cols = Physics.OverlapSphere(explosionPos, radius);  // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it

            /* for each collider triggered, damages and EXPLOSION force are applied */
            foreach (Collider col in cols)
            {
                if (col.gameObject.tag == "Player" || col.gameObject.GetComponent<EntityLivingBase>() && !col.isTrigger && !col.GetComponent<PetSummoner>()) // if the target triggered is an Enemy monster and the collider is not a trigger apply damages and status
                {
                    Debug.Log(col);
                    if (col.gameObject.tag != "Player") // if the collider is own by an EntityLivingBase
                    {
                        col.gameObject.GetComponent<EntityLivingBase>().DamageFor(ExplosionDamages); // apply damages to triggered targets Monster Player and pet, damages value is depending of the exponential fonction of the number of monsters with the touch status
                        Debug.Log(ExplosionDamages + " Damages applied to Enemy Monster, PNJ and Player");
                    }

                    Rigidbody rb = col.GetComponent<Rigidbody>(); // instanciation of rigibody of the targets caught in the EXPLOSION
                    if (rb != null)
                    {
                        rb.AddExplosionForce(power, explosionPos, radius, 3.0F); // projection force applied on the rigibodies caught in the EXPLOSION 
                        Debug.Log("EXPLOSION FORCE");
                    }
                }
            }
            /* remove all TouchStatus from the EnemyMonster on the map */
            foreach (TouchStatus TargTouch in GetComponentInParent<DeflagrationSpell>().TargetsTouched)
                TargTouch.DestroyStatus();

            //Destroy(Rigidbody); // destruction of the Player's rigidbody
            Destroy(GetComponentInParent<SummonerInterface>().Pet); // TOTAL DESTRUCTION OF THE PET !!!!!!
            GetComponentInParent<NeutralFormSpell>().IsSpellUsable = true;
            Debug.Log("PET Destroy");
            Debug.Log("Retour en forme Neutre and apply a status to the Player taht prevent him to cast invocation Spell during 1 paintfull min");
            //GetComponentInParent<NeutralFormSpell>().LaunchSpell();    
        }
        else
            return;
    }
}
