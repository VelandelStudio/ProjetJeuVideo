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
            Debug.Log("sort lancé");
            // Invoke("Ally_monster", 2);
            pospet = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z + 2);
            Instantiate(PetMonsterAOE, pospet, Quaternion.identity);
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

   /* protected override void Update()
    {
        if (count == 2)
        {
            Destroy(PetMonsterAOE);
        }
    }*/
}

