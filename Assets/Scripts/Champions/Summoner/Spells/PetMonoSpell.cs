using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** PetMonoSpell, public class
 * @extends : spell
 * This spell is associated with the Neutral's form of the summoner.
 * This spell allows the summoner to go to the MonoForm. 
 **/
public class PetMonoSpell : Spell
{


    //private GameObject PetMonster = (GameObject)Resources.Load("SummonerNeutral/PetMonster");
    private GameObject _pet;
    private GameObject _target;
    private GameObject _swapMonoPS;
    public Vector3 _posPet;
    // private GameObject SummonerNeutral = (GameObject)Resources.Load("Champion/SummonerNeutral");

    /// <summary>
    /// Override Start method
    /// Load the wall from the ressources and call the mother method
    /// </summary>
    protected override void Start()
    {
        _pet = LoadResource("ElementalMono");
        _swapMonoPS = (GameObject)Resources.Load("ParticleSystems/SummonerMonoInvoke/SwapSummonerMonoPS");

        base.Start();
    }

    public int count = 0;

    /** LaunchSpell, public override void method
    * When launched this PetMonoSpell we instantiate the SummonerMono champion in the oldChampion's position and we instantiate the PetAOE in the old pet's position.
    * The default target of the new pet is the newChampionObj but if the target of the old pet was another gameObject, this one becomes the target of the new pet.
    * Finally we destroy the old champion and the old pet.
    **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {

            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerMono");
            Camera.main.transform.parent = null;
            newChampionObj = Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Instantiate(_swapMonoPS, newChampionObj.transform);

            Debug.Log("Hello world");
            _posPet = new Vector3(transform.position.x + 2, transform.position.y + 1, transform.position.z + 2);
            _target = newChampionObj;

            if (GetComponent<SummonerInterface>().Pet != null)
            {
                _posPet = GetComponent<SummonerInterface>().Pet.transform.position;

                if (GetComponent<SummonerInterface>().Pet.GetComponent<PetSummoner>().Target != gameObject)
                {
                    _target = GetComponent<SummonerInterface>().Pet.GetComponent<PetSummoner>().Target;
                }

                Destroy(GetComponent<SummonerInterface>().Pet);
                Debug.Log("Destroy");
            }

            
            Debug.Log("sort lancé");
            
            _pet = Instantiate(_pet, _posPet, Quaternion.identity);
            _pet.GetComponent<PetSummoner>().Target = _target;
            _pet.GetComponent<PetSummoner>().Summoner = newChampionObj;
            newChampionObj.GetComponent<SummonerInterface>().Pet = _pet;

            Destroy(oldChampion.gameObject);
            base.OnSpellLaunched();

        }

    }
}
