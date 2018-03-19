using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//big BOUM !!!!!!!! => apply heavy damages on everything with HP in the area destroy GameObject pet then load summonerNeutral (or launch NeutralFormSpell)

public class AnnihilationSpell : Spell
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        //TargetsTouched.RemoveAll(TouchStatus => TouchStatus == null); // Remove all null TouchStatus.
        base.Update();
    }


    public override void LaunchSpell()
    {
        if (IsSpellLauncheable())
        {
         
            if (GetComponent<SummonerInterface>().Pet!=null) //if the PetAOE is alive then the spell is cast
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
            ApplyStatus(GetComponent<AnnihilationSpell>().Status[1], GetComponentInChildren<SummonerInterface>().Pet.transform); // CountDownBeforetheBoomStatus is applied to the Pet

            base.OnSpellLaunched();

        }
    }
        
}
