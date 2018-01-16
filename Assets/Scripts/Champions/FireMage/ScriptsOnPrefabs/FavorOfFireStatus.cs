using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** FavorOfFire, public class
 * @extends : StatusBase
 * @implements : IBuff
 * Status attached to the FavorOfFire Prefab, launched by the FireBlessingSpell of the FireMage Champion
 * This Status allows the next Conflagration of the FireMage to have 100% chances to propagate ignites.
 * Lasts for 5 sec. max.
 **/
public class FavorOfFireStatus : StatusBase, IBuff
{

    private ConflagrationSpell _conflagration;

    /** OnStatusApplied, public override void :
     * Launched by the base.Start automatically.
     * When launched, get the ConflagrationSpell component and set the CritSuccessto true.
     **/
    public override void OnStatusApplied()
    {
        _conflagration = GetComponentInParent<ConflagrationSpell>();
        _conflagration.CritSuccess = true;
    }

    public override void StatusTickBehaviour() { }

    /** DestroyStatus public virtual void
     * Instantly Destroy the gameObject that contains the Status
     * And re-set the Conflagration CritSuccess to false.
     **/
    public override void DestroyStatus()
    {
        _conflagration.CritSuccess = false;
        base.DestroyStatus();
    }
}