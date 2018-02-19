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

    public override void LaunchSpell()
    {

        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            Debug.Log("sort lancé");
           // Invoke("Ally_monster", 2);           
           Instantiate(PetMonsterAOE, new Vector3(-75, 275, -75), Quaternion.identity);
            base.OnSpellLaunched();       
        }
    }
    /* LoadResource, protected virtual GameObject Method
     * @param : string,
     * @return : GameObject
     * This method is used to load a GameObject prefab inside the champion folder.
     */
    /*  protected virtual GameObject LoadResource(string prefabName)
      {
          return (GameObject)Resources.Load(champion.Name + "/" + prefabName);
      }*/

    protected override void Update()
    {
        if (count == 2)
        {
            Destroy(PetMonsterAOE);
        }
    }
}

