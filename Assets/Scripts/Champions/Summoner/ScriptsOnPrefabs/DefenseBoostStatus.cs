using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBoostStatus : StatusBase, IBuff
{
    private float DefenseToAdd;
    private float DefenseAdded=0;

    protected override void Start()
    {
        base.Start();
        transform.localPosition += new Vector3(0f, transform.parent.lossyScale.y * 2f, 0f);
    }

    /* status that boost the defense, it's applied on the summonerAOE and his PET by the PassiveSummonerPetAOE */
    public override void OnStatusApplied()
    {
        DefenseToAdd = (float)System.Math.Round(double.Parse(OtherValues[0]), 1); // Defense to add each time the status is called
        DefenseAdded += DefenseToAdd; //
        // value of defense to increase depending of the number of monsters with TouchStatus
        if (transform.parent.tag == "Player")
        {
            this.characteristics.Defense += DefenseToAdd;
            Debug.Log("Player DefenseToAdd: " + DefenseToAdd);
            Debug.Log("Summoner characteristics.Defense : " +characteristics.Defense);
        }
        else
        {
            Debug.Log("Pet characteristics.Defense avant boost : " + characteristics.Defense);
            this.characteristics.Defense += DefenseToAdd;
            Debug.Log("Pet DefenseToAdd: " + DefenseToAdd);
            Debug.Log("Pet characteristics.Defense après boost :" + characteristics.Defense);
        }
 
    }

    private void OnDestroy()
    {
        characteristics.Defense -= DefenseAdded;
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}


