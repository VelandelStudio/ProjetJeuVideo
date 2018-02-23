using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetAOESpell : Spell {

    private GameObject PetMonsterAOE;

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
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerAOE");
<<<<<<< HEAD
            newChampionObj=Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Destroy(oldChampion.gameObject);
           
=======
            newChampionObj=Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation); // newChampionObj becomes GameObject in the scene not in the prefab !!!!!
           
            //gameObject.SetActive(false);

            Debug.Log("sort lancé");
            pospet = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            Instantiate(PetMonsterAOE, pospet, Quaternion.identity, newChampionObj.transform);


            Destroy(oldChampion.gameObject);
            base.OnSpellLaunched();
>>>>>>> b39b5003a40313e22830028c39d2b0e9c2831ea7
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

