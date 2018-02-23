using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PetSupportSpell : Spell {

    private GameObject PetMonsterSupp;
    private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");
    private GameObject PetMonster;


    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
    
        PetMonsterSupp = LoadResource("PetMonsterSupp");
        base.Start();
    }
    public Vector3 pospet;

    public override void LaunchSpell()
    {

        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
        
            Debug.Log("sort lancé");
            pospet = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            Instantiate(PetMonsterSupp, pospet, Quaternion.identity);
            //Pet = true;
            base.OnSpellLaunched();
            //change de forme 
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerSupport");
            newChampionObj=Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Destroy(oldChampion.gameObject);
          
        }
    }
    /*protected override void Update()
    {

        base.Update();
        PetMonsterSupp.transform.position = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 2);
        // PetMonsterSupp.transform.position = SummonerNeutral.transform.position;
        //MonsterMove();
    
}*/
}
