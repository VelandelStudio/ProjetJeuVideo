using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FireBlessingSpell Class, extends Spell
 * This spell is associated with the FireMageClass
 * The objectif of this spell is to set the Critic Success of the Conflagration Spell at 100%
 **/
public class FireBlessingSpell : Spell
{
    private GameObject _fireBlessingStatus;
    private int _durationOfCritSuccess;

    /** Start : protected override void Method
	 * We launch the mother Method to initialize the spell.
	 **/
    protected override void Start()
    {
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
        _durationOfCritSuccess = int.Parse(OtherValues[0]);
        GameObject fireBlessingInst = ApplyStatus(Status[0], transform);
        base.OnSpellLaunched();
    }
}