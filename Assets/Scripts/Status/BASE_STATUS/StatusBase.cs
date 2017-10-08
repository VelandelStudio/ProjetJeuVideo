using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** StatusBase public abstract class
 * Implements IStatus.
 * This class is the mother class of most of the buffs/debuffs/Status in our game.
 * It aims to simplify the creation of Status, using the Template pattern.
 **/
public abstract class StatusBase : MonoBehaviour, IStatus {

    protected int maxDuration;
    protected float tickInterval;
    protected float delay;

    /** Start protected virtual void
     * The method is here to launch the method OnStatusApplied.
     * Then, it lanches an InvokeRepeating on StatusTickBehaviour to make the status tick every tickInterval;
     * Finally, it lanches an Invoke on DestroyStatus that will occurs in maxDuration seconds.
     **/
    protected virtual void Start()
    {
        OnStatusApplied();
        InvokeRepeating("StatusTickBehaviour", delay, tickInterval);
        Invoke("DestroyStatus", maxDuration);
    }

    /** OnStatusApplied public abstract void
     * Note that this method is abstract and should always be implemented in the extenders.
     * The objective is to satisfy the Template pattern. In that way, the Start method will automatically call the OnStatusApplied in the inherited script.
     **/
    public abstract void OnStatusApplied();

    /** StatusTickBehaviour public abstract void
     * Note that this method is abstract and should always be implemented in the extenders.
     * The objective is to satisfy the Template pattern. In that way, the Start method will automatically call the StatusTickBehaviour in the inherited script.
     **/
    public abstract void StatusTickBehaviour();

    /** ResetStatus public virtual void
     * This method should be used when we want to Reset the debuff. In this way, we do not need to Destroy an instance of the debuff and then create a new one.
     * In order to do that, we call the OnStatusApplied Method to apply the fresh new inputs from inherited members.
     * Then we cancel and re-Invoke StatusTickBehaviour and DestroyStatus Methods
     **/
    public virtual void ResetStatus()
    {
        OnStatusApplied();
        CancelInvoke("StatusTickBehaviour");
        CancelInvoke("DestroyStatus");
        InvokeRepeating("StatusTickBehaviour", delay, tickInterval);
        Invoke("DestroyStatus", maxDuration);
    }

    /** DestroyStatus public virtual void
     * Instantly Destroy the gameObject that contains the Status
     **/
    public virtual void DestroyStatus()
    {
        Destroy(gameObject);
    }
}
