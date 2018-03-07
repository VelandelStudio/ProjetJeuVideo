using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationStatus : StatusBase, IBuff {

    protected override void Start()
    {
        base.Start();
        //transform.localPosition += new Vector3(0f, transform.parent.lossyScale.y * 2f, 0f);
    }



    /* status that boost the life point , it's applied on the summonerSupport and his PET by the PassiveSummonerPetSupport*/
    public override void OnStatusApplied()
    {

        if (transform.parent.tag == "Player")
        {
            Debug.Log(int.Parse(OtherValues[0]+" regen of player "));
            // ajouter de la vie au player 
        }
        else
        {
            GetComponent<PetSummoner>().HealFor(int.Parse(OtherValues[1]));

            Debug.Log(int.Parse(OtherValues[1])+" regen of pet");
        }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
