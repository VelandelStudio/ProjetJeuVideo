using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** NeutralFormSpell, public class
 * @extends : spell
 * This spell is associated with the Mono, AOE and support forms of the summoner.
 * This spell allows theses forms to back to NeutralForm. 
 **/

public class NeutralFormSpell : Spell
{

    private GameObject _petNew;
    private GameObject _petOld;
    private System.Type _type;

    public Vector3 _posPet;
    private GameObject _swapNeutralPS;

    protected override void Start()
    {
        _swapNeutralPS = (GameObject)Resources.Load("ParticleSystems/SummonerNeutralInvoke/SwapSummonerNeutralPS");
        base.Start();
    }
    /** LaunchSpell, public override void method
     * When launched this NeutralFormSpell we instantiate the SummonerNeutral champion in the oldChampion's position.
     * We check if the oldChampion have a pet associated and in this case we duplicate this pet and destroy the old one.
     * The old pet's target is checked in order to give the newChampionObj as target at the new pet if oldChampion was the old pet's target.
     * In other case we give the current old pet's target at the new pet.
     **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            GameObject oldChampion = Camera.main.transform.parent.gameObject;
            GameObject newChampionObj = (GameObject)Resources.Load("Champions/" + "SummonerNeutral");
            Camera.main.transform.parent = null;
            newChampionObj = Instantiate(newChampionObj, oldChampion.transform.position, oldChampion.transform.rotation);
            Instantiate(_swapNeutralPS, newChampionObj.transform.position, newChampionObj.transform.rotation, newChampionObj.transform);

            _petOld = oldChampion.GetComponent<SummonerInterface>().Pet;
            _posPet = new Vector3(transform.position.x + 2, transform.position.y + 1, transform.position.z + 2);

            if (_petOld != null)
            {
                _posPet = _petOld.transform.position;
                _petNew = Instantiate(_petOld, _posPet, Quaternion.identity);
                newChampionObj.GetComponent<SummonerInterface>().Pet = _petNew;

                if (_petOld.GetComponent<PetSummoner>().Target == gameObject)
                {
                    _petNew.GetComponent<PetSummoner>().Target = newChampionObj;
                }

                else
                {
                    _petNew.GetComponent<PetSummoner>().Target = _petOld.GetComponent<PetSummoner>().Target;
                }

                Destroy(_petOld);
            }


            Destroy(oldChampion.gameObject);
            Debug.Log("sort lancé");

           base.OnSpellLaunched();
        }
    }
}
