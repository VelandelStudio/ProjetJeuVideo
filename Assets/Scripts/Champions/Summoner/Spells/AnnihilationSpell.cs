using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//big BOUM !!!!!!!! => apply heavy damages on everything with HP in the area destroy GameObject pet then load summonerNeutral (or launch NeutralFormSpell)
public class AnnihilationSpell : Spell
{
    bool ReadyToExplose;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        //TargetsTouched.RemoveAll(TouchStatus => TouchStatus == null); // Remove all null TouchStatus.
        ReadyToExplose = GetComponentInChildren<AllyMonster>().GetComponent<AnnihilationSpell>().IsSpellInUse() && GetComponentInChildren <AllyMonster>().GetComponent<AnnihilationSpell>().Status[1] == null;
        base.Update();
    }


    public override void LaunchSpell()
    {
        if (IsSpellLauncheable())
        {

            if (GetComponentInChildren<AllyMonster>().IsAlive) //if the PetAOE is alive then the spell is cast
            {
                base.LaunchSpell();
                Debug.Log("sort lancé");
            }
            else
            {
                Debug.Log("The Pet IS DEAD so he can't SUICIDE");
                return;
            }


            ApplyStatus(GetComponent<AnnihilationSpell>().Status[0], transform); // WaitForTheBoomStatus is applied to the player
            ApplyStatus(GetComponent<AnnihilationSpell>().Status[1], GetComponentInChildren<AllyMonster>().transform); // CountDownBeforetheBoomStatus is applied to the Pet

            if (ReadyToExplose)
            {
                Collider[] cols = Physics.OverlapSphere(GetComponentInChildren<AllyMonster>().transform.position, 5f); // Create an OverlapSphere that recup the list of the EnemyMonster colliders that triggered it.

                /* for each collider (capsule collider only) triggered, damages and TouchStatus are applied */
                foreach (Collider col in cols)
                {
                    if (!col.isTrigger) // if the target triggered is an Enemy monster and the collider is not a trigger apply damages and status
                    {

                        col.gameObject.GetComponent<EntityLivingBase>().DamageFor(Damages[0]); // apply damages to triggered targets Monster Player and pet
                        //col.gameObject.GetComponent<CharacterController>().DamageFor(Damages[0]); // apply damages to triggered targets
                        Debug.Log("Damages applied to target");
                    }
                }
            }

            base.OnSpellLaunched();

        }
    }
        
}
