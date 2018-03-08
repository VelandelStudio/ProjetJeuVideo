using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** EscapeSpell, public class
 * @extends : Spell
 * This scrit is associated with the SummonerNeutral.
 * It allow to the pet to come back to the summoner and apply a status on. 
 **/
public class EscapeSpell : Spell
{

    private GameObject _summoner;
    private GameObject _pet;
    private bool _spellIsUsed;


    /** Start, protected override void
     * Start get the summoner's gameObject and pet's gameObject and initialize the _spellIsUsed variable.
     **/
    protected override void Start()
    {
        _summoner = gameObject;
        _pet = gameObject.GetComponent<SummonerInterface>().Pet;

        _spellIsUsed = false;

        base.Start();
    }

    /**Update, protected override void method
     * Each frame the game checks if the pet is in range to apply the status and if the spell is not already used. 
     **/
    protected override void Update()
    {

        if (_pet != null && Vector3.Distance(_pet.transform.position, _summoner.transform.position) < 5.0f && _spellIsUsed == true)
        {
            ApplyEffect(_summoner);
            _spellIsUsed = false;
        }

        base.Update();
    }

    /** LauchSpell, public override void method
     * This method is called when the player launch his spell and allows to switch the target to summoner and changes _spellIsUsed to true
     **/
    public override void LaunchSpell()
    {
        if (_pet != null)
        {

            base.LaunchSpell();
            if (IsSpellLauncheable())
            {
                _pet.GetComponent<PetSummoner>().Target = _summoner;
                _spellIsUsed = true;

                Debug.Log("sort lancé");
                base.OnSpellLaunched();
            }
        }
    }

    /** ApplayEffect, public void method
     * This method is called when the distance between summoner and pet is lower than the distance define in the Update method. 
     **/
    public void ApplyEffect(GameObject gameObject)
    {
        ApplyStatus(Status[0], gameObject.transform);
    }
}
