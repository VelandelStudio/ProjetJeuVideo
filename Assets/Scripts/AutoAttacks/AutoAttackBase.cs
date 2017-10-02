using UnityEngine;

/** AutoAttackBase abstract class.
 * This abstract class is the mother class of all AutoAttack in our game. 
 * This class handles the behaviour the CD of all AutoAttacks. It also contains the AutoAttack method launched by the Classe.
 **/
public abstract class AutoAttackBase : MonoBehaviour
{
    protected float GCD;
    protected float CurrentGCD;

    /** Start protected virtual void Method,
	 * The Start method initializes the CD of the auto-attack.
	 **/
    protected virtual void Start()
    {
        CurrentGCD = GCD;
    }

    /** Update protected virtual void Method,
	 * If the auto-attack is not ready (i.e. if it has already been launched), then the current CD is reseting, time after time.
	 **/
    protected virtual void Update()
    {
        if (!AutoAttackIsReady())
            CurrentGCD = Mathf.Clamp(CurrentGCD + Time.deltaTime, 0, GCD);
    }

    /** AutoAttackIsReady protected bool Method,
	 * This returns if the auto-attack is launchea&ble or not. In this script, we only check if the auto-attack is under Cooldown or not.
	 **/
    protected bool AutoAttackIsReady()
    {
        return (CurrentGCD == GCD);
    }

    /** AutoAttack public virtual void Method,
	 * This public Method should always be called by the Classe script. 
	 * When the Auto-attack is launched, the GCD is set to zero.
	 * Please note that, there is no text displayed when the auto-attack is under cooldown if the player tried to launche it.
	 * This is volunteer, because if the player hold the mouse button down, this will spamm the Debug log and next, the player screen with messages.
	 **/
    public virtual void AutoAttack()
    {
        CurrentGCD = 0;
    }
}