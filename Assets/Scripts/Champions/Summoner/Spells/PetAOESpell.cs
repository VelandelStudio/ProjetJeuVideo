using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAOESpell : Spell {

    private GameObject PetMonsterAOE;
    private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        PetMonsterAOE = LoadResource("PetMonsterAOE");
        base.Start();
    }
    public int count = 0;
    public Vector3 pospet;

    public override void LaunchSpell()
    {

        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
            pospet = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            Instantiate(PetMonsterAOE, pospet, Quaternion.identity);
            base.OnSpellLaunched(); 
            //change de forme   
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerAOE");
            Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Destroy(oldChampion.gameObject);
            gameObject.SetActive(false);
        }
    }
   

   /* protected override void Update()
    {
        if (count == 2)
        {
            Destroy(PetMonsterAOE);
        }
    }*/
}

