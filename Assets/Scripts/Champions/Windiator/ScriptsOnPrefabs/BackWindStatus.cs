using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** BackWindStatus, public class
 * @extends StatusBase
 * @implements IDebuff
 * This Status is used by the Windiator Champion.
 * When applied, the projectile or the monster is slowed down (move speed) during a certain amount of time.
 * After that, he recovers its orginal move speed.
 **/
public class BackWindStatus : StatusBase, IDebuff
{
    IProjectile projectile;

    /** OnStatusApplied, public override void
	 * If we get a projectile, we launch the SlowDownProjectile method.
	 **/
    public override void OnStatusApplied()
    {
        projectile = transform.GetComponentInParent<IProjectile>();
        if (projectile != null)
        {
            projectile.SlowDownProjectile(float.Parse(OtherValues[0]));
        }
    }

    /** DestroyStatus public virtual void
	 * Before everuthing, we reset the move speed of the GameObject associated.
     * Instantly Destroy the gameObject that contains the Status
     **/
    public override void DestroyStatus()
    {
        if (projectile != null)
        {
            projectile.AccelerateProjectile(float.Parse(OtherValues[0]));
        }
        base.DestroyStatus();
    }

    public override void StatusTickBehaviour() { }

}