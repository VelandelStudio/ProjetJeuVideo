using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** VelocityStatus Class 
 * @extends StatusBase
 * This Status is associated with the prefab of the VelocityStatus
 * This script inscreases the move speed of the player when it is applied
 **/
public class VelocityStatus : StatusBase, IBuff
{

    /** OnStatusApplied, public override void Method
     * Called when the VelocityStatus is applied and increases the move speed of the player.
    **/
    public override void OnStatusApplied()
    {
        Debug.Log("Player's move speed : " + this.characteristics.MovementSpeedFactor);
        this.characteristics.MovementSpeedFactor += float.Parse(OtherValues[0]);
        Debug.Log("Player's move speed : " + this.characteristics.MovementSpeedFactor);
    }

    /** DestroyStatus, public override void Method
     * Called when the duration of the status is over.
     * This method decrease the move speed of the player and destroy the gameObject.
    **/
    public override void DestroyStatus()
    {
       this.characteristics.MovementSpeedFactor -= float.Parse(OtherValues[0]);
        Debug.Log("Player's move speed : " + this.characteristics.MovementSpeedFactor);

        base.DestroyStatus();
    }

    public override void StatusTickBehaviour()
    {
        throw new System.NotImplementedException();
    }
}
