using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSummonerPetAOE : PassiveBase {

    float dist;
    int nbEnemyMonsterTouched;
    int touchCount;

    public int TouchCount
    {
        get { return touchCount; }
        set {
            touchCount = value;
            ApplyDefenseBoost(); // if the touch count is modified by the TouchStatus on a Monster ( if a monster get the TouchStatus or Reset it) then called the method ApplyDefenseBoost that reapply apply DefenseBoostPlayerStatus to the Player and DefenseBoostPetStatus to the Pet with the defenseToAdd value updated
        }
    }

    // Update is called once per frame
    void Update()
    {
        nbEnemyMonsterTouched = GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count;

        if (GetComponent<SummonerInterface>().Pet != null)//if the PetAOE is alive then we calculate the distance between him and the Player and use the passive
        { 
            dist = Vector3.Distance(GetComponent<SummonerInterface>().Pet.transform.position, transform.position); // process the distance between the SummonerAOE and his Pet

            /* if the SummonerAOE (Player) is in a certain range of his Pet and there are Monster "touched" and there's no DefenseBoosPlayertStatus or DefenseBoostPetStatus  already applied then apply DefenseBoostPlayerStatus to the Player and DefenseBoostPetStatus to the Pet */
            if (dist <= float.Parse(OtherValues[0]) && nbEnemyMonsterTouched >0 && !FindObjectOfType<DefenseBoostPlayerStatus>())
            {
                ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], transform); // Apply DefenseBoostPlayerStatus  the Player
            }
            if (dist <= float.Parse(OtherValues[0]) && nbEnemyMonsterTouched > 0 && !FindObjectOfType<DefenseBoostPetStatus>())
            {
                ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[1], GetComponent<SummonerInterface>().Pet.transform); //  Apply DefenseBoostPetStatus  the Pet
            }
            // if Player has SummonerMono stack apply DamageBoostStatus (not created yet)
            /* if (dist <= float.Parse(OtherValues[0]) && Player has stack of summonerMono form){
             * 
             * ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[2], transform); // Status[2] DamageBoostStatus not created yet
             * 
             }*/
        }
    }

    private void ApplyDefenseBoost() 
    {
        nbEnemyMonsterTouched = GetComponentInParent<DeflagrationSpell>().TargetsTouched.Count;

        if (GetComponent<SummonerInterface>().Pet != null)//if the PetAOE is alive then we calculate the distance between him and the Player and use the passive
        {
            dist = Vector3.Distance(GetComponent<SummonerInterface>().Pet.transform.position, transform.position); // process the distance between the SummonerAOE and his Pet

            /* if the SummonerAOE (Player) is in a certain range of his Pet and there are Monster "touched" and there's no DefenseBoosPlayertStatus or DefenseBoostPetStatus  already applied then apply DefenseBoostPlayerStatus to the Player and DefenseBoostPetStatus to the Pet */
            if (dist <= float.Parse(OtherValues[0]) && nbEnemyMonsterTouched > 0 && !FindObjectOfType<DefenseBoostPlayerStatus>())
            {
                ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[0], transform); // Apply DefenseBoostPlayerStatus  the Player
            }
            if (dist <= float.Parse(OtherValues[0]) && nbEnemyMonsterTouched > 0 && !FindObjectOfType<DefenseBoostPetStatus>())
            {
                ApplyStatus(GetComponent<PassiveSummonerPetAOE>().Status[1], GetComponent<SummonerInterface>().Pet.transform); //  Apply DefenseBoostPetStatus  the Pet
            }
        }
    }
}
