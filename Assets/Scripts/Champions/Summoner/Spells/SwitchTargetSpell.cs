using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** SwitchTargetSpell, public class.
 * @extends : Spell.
 * This spell is associated to all Summoner's forms.
 * This spell allows to the summoner to switch the pet's target.
 * This class 
 **/
public class SwitchTargetSpell : Spell
{
    private GameObject _pet;
    private GameObject _summoner;
    private GameObject _target;

    private Ray _shootRay = new Ray();
    private RaycastHit _shootHit;
    private int _shootableMask;


    public float range = 1000f;

    /** LaunchSpell, public override void method,
	 * When launched, we use a raycast which ignores trigger. When the raycast hits a collider we check if the hited gameObject's tag equals to Monster. 
     * If the gameObject hited is'nt a monster the target is the summoner himself.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();
        if (IsSpellLauncheable())
        {
            _pet = GetComponentInParent<SummonerInterface>().Pet;



            _shootableMask = LayerMask.GetMask("Default");

            Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);

            if (Physics.Raycast(Camera.main.transform.position, forward, out _shootHit, range, _shootableMask, QueryTriggerInteraction.Ignore))
            {
                _target = _shootHit.transform.gameObject;
                Debug.Log(this.gameObject);

                if (_target.tag == "Monster")
                {
                    Debug.Log("RaycastHit : " + _target);
                    _pet.GetComponent<PetSummoner>().Target = _target;

                }

                else
                {
                    _pet.GetComponent<PetSummoner>().Target = gameObject;
                }
            }



            Debug.Log("RaycastHit : " + _target);
            Debug.Log("sort lancé");

            base.OnSpellLaunched();
        }
    }
}
