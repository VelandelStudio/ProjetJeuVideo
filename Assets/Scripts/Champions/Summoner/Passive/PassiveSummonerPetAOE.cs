using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerPetAOE : PassiveBase {

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(GetComponentInChildren<EntityLivingBase>().transform.position, transform.position); // process the distance between the SummonerAOE and his Pet
        /* if the SummonerAOE (Player) is in a certain range of his Pet apply DefenseBoostStatus to the Player and the Pet */
        if(dist<=float.Parse(OtherValues[0]) && GetComponentInChildren<DeflagrationSpell>().TargetsTouched.Count>0)
        {
           // ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], GetComponentInChildren<AllyMonster>().transform); Problem with the GUi the Pet being children of the his Summoner
           ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], transform);
        }
    }
}
