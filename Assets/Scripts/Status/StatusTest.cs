using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** StatusTest public class
 * Extends StatusBase, Implements IStatus (via mother class).
 * This class should always be attached to a GameObject that represent the Status.
 * This Test Class represents a buff/Debuff that stands for 10 sec and has an effect every sec.
 **/
public class StatusTest : StatusBase
{
    /** OnStatusApplied, public override void
     * Method from interface. This method is used to apply the correct property of our Status.
     **/
    public override void OnStatusApplied()
    {
        Debug.Log("StatusTest Created !");
    }

    /** StatusTickBehaviour, public override void
     * Method from interface. This method is launched every tickInterval by the mother class.
     **/
    public override void StatusTickBehaviour()
    {
        Debug.Log("StatusTest Tick !");
    }
}
