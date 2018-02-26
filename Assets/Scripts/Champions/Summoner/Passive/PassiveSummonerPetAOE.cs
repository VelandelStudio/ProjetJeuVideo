using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerPetAOE : PassiveBase {

    float dist;
    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(GetComponent<SummonerAOE>().Pet.transform.position, transform.position); // process the distance between the SummonerAOE and his Pet

        /* if the SummonerAOE (Player) is in a certain range of his Pet apply DefenseBoostStatus to the Player and the Pet */
        if (dist <= float.Parse(OtherValues[0]) && GetComponentInChildren<DeflagrationSpell>().TargetsTouched.Count > 0)
        {
            //ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], GetComponent<SummonerAOE>().Pet.transform); //Problem with the GUi the Pet being children of the his Summoner it shoud not be a problem now !
            ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], transform); // Apply DefenseBoostStatus to Player
        }
        // if Player has SummonerMono stack apply DamageBoostStatus (not created yet)
        /*else if (dist <= float.Parse(OtherValues[0]) && Player has stack of summonerMono form){
         * 
         * ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[1], transform); // Status[1] DamageBoostStatus not created yet
         * 
         }*/
    }
}
