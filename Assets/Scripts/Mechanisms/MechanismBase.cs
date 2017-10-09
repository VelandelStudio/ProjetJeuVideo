using UnityEngine;

/** MechanismBase Script
 * @Requires Collider
 * This abstract class is the base for all mechanisms in the game. 
 * Mechanisms are GameObjects that can be activated only once such as lever, doors, whatever...
 * This class must be inherited by a class attached to a GameObject with a collider to intercept Raycasts launches by the GameObjectDetector class attached to player.
 **/
 [RequireComponent(typeof(Collider))]
public abstract class MechanismBase : MonoBehaviour, IInterractableEntity
{
    protected bool isActivated;

    /** Start Method
     * Set the mechanism activable and not activated yet.
     **/
    protected virtual void Start() {
        isActivated = false;
    }

    /** ActivateInterractable Method
     * Virtual method. Every mechanisms must override this method and call this method to tell the game the mechanism has already been activated.
     * This method is called when the player activates a mechanism.
     **/
    public virtual void ActivateInterractable() {
        Debug.Log("Mechanism base activated");
        isActivated = true;
    }

    /** DisplayTextOfInterractable Method
     * Virtual method. This method doesn't necessarily need to be overrided.
     * It provides a text to display when the mechanism is activable
     * FEATURE TO ADD -> A.T.M. Displays the text in the DebugConsole - SOON : Displays the text on a GUI Screen
     **/
    public virtual void DisplayTextOfInterractable() {
        if (!isActivated)
            Debug.Log("Press " + InputsProperties.activate.ToString() + " to activate.");
    }
}
