using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FireBlessingSpell Class, extends Spell
 * This spell is associated with the FireMageClass
 * The objectif of this spell is to set the Critic Success of the Conflagration Spell at 100%
 **/
public class FireBlessingSpell : Spell
{
    private ConflagrationSpell _conflagration;
    private float _durationOfCritSuccess = 5.0f;

    /** Start : protected override void Method
	 * The Start Method is used here to get the set the CD of the spell.
	 * Then, the scripts is looking for the ConflagrationSpell associated to the player.
	 * Once it is done, we launch the mother Method to initialize the spell.
	 **/
    protected override void Start()
    {
        SpellCD = 30.0f;
        _conflagration = GetComponent<ConflagrationSpell>();
        base.Start();
    }

    /** LaunchSpell : public override void Method
	 * The LauncheSpell Method is called by the abstract Class Classe when the player press the key associated to the spell.
	 * First at all, we launch the mother method to initialize the spell launching. If the spell is Launcheable, we set the Conflagration.CritSuccess to true.
	 * This will Invoke the CancelFireBlessingSpell Method of this script in durationOfCritSuccess seconds and display a text on the screen (Debug.Log).
	 * Finally, we call the OnSpellLaunched method in the mother class.
	 **/
    public override void LaunchSpell()
    {
        base.LaunchSpell();

        if (!IsSpellLauncheable())
        {
            return;
        }

        _conflagration.CritSuccess = true;
        Debug.Log("Conflagration CritSuccess 100% for 5 sec !");
        Invoke("CancelFireBlessingSpell", _durationOfCritSuccess);
        base.OnSpellLaunched();
    }

    /** CancelFireBlessingSpell : private void Method
	 * This Method reset the conflagration.CritSuccess from true to false when the buff fades. 
	 * Please note that when the method is launched, if the conflagration.CritSuccess is already set to false (because the Conflagration was used), nothing happens.
	 **/
    private void CancelFireBlessingSpell()
    {
        if (_conflagration.CritSuccess)
        {
            _conflagration.CritSuccess = false;
            Debug.Log("Conflagration CritSuccess ended !");
        }
    }
}
