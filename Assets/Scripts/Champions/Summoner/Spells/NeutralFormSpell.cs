using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralFormSpell : Spell
{
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            //load GameObject(SummonerNeutral)
            Debug.Log("sort lancé");
            base.OnSpellLaunched();
            //change de forme 
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerNeutral");
            newChampionObj=Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Destroy(oldChampion.gameObject);
        }
    }
}
