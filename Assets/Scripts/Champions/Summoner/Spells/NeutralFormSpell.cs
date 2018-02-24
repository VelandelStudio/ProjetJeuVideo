using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeutralFormSpell : Spell
{

    private GameObject _pet;
    private System.Type _type;
    public Vector3 _posPet;

    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerNeutral");
            newChampionObj = Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
           // Destroy(oldChampion.gameObject);

            _posPet = new Vector3(transform.position.x + 2, transform.position.y + 1, transform.position.z + 2);

            if (GetComponent<SummonerInterface>().Pet != null)
            {
                _posPet = oldChampion.GetComponent<SummonerInterface>().Pet.transform.position;
                _pet = Instantiate(oldChampion.GetComponent<SummonerInterface>().Pet, _posPet, Quaternion.identity);
                newChampionObj.GetComponent<SummonerInterface>().Pet = _pet;

                if (GetComponent<SummonerInterface>().Pet.GetComponent<PetSummoner>().Target == gameObject)
                {
                    _pet.GetComponent<PetSummoner>().Target = newChampionObj;
                }

                else
                {
                    _pet.GetComponent<PetSummoner>().Target = GetComponent<SummonerInterface>().Pet.GetComponent<PetSummoner>().Target;
                }

                Destroy(oldChampion.GetComponent<SummonerInterface>().Pet);
            }


            Destroy(oldChampion.gameObject);
            //load GameObject(SummonerNeutral)
            Debug.Log("sort lancé");


            //change de forme 


            base.OnSpellLaunched();
        }
    }
}
