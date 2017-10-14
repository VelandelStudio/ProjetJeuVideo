using UnityEngine;

/** Shield public class.
 * This script should always be associated with en EntityLivingBase.
 * It applies on the target a shield that will be consummed before its HP.
 **/
public class Shield : MonoBehaviour
{
    public float ShieldValue { get; private set; }

    /** AddShieldValue public void
	 * @Param : float value
	 * This Method is called by other elements in the game to add points to the shield.
	 **/
    public void AddShieldValue(float shieldValue)
    {
        this.ShieldValue += shieldValue;
    }

    /** ReduceShieldValue public void
	 * @Param : float value
	 * This Method is called by other elements in the game to remove points to the shield.
	 * If the shieldValue reaches 0, the RemoveShield Method is launched.
	 **/
    public void ReduceShieldValue(float shieldValue)
    {
        ShieldValue -= shieldValue;
        if (ShieldValue <= 0)
        {
            RemoveShield();
        }
    }

    /** RemoveShield public void
	 * Instantly Destroy this script.
	 **/
    public void RemoveShield()
    {
        Destroy(this);
    }
}