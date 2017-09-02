using UnityEngine;

/** GameObjectDetector Script
 * Launch a RayCastAll from the GameObject to the forward direction of this GameObject.
 * The RayCastAll is a short distance ray made to represent if the player is close enough to something interractable.
 * Only use with the player fps cam is recommended.
 **/
public class GameObjectDetector : MonoBehaviour
{
    [SerializeField]
    private GameObject eyes;

    private RaycastHit[] hitInfo;

    /** FixedUpdate Method 
     * Get the distance between the camera and the player eyes then draw a DebugRay from the center of the camera to a position forward.
     * The maxDistance of the ray was fixed arbitrary to characterDistance*1.5f. In that way, if the players dezoom the camera, the max distance remains the same.
     * Afer that, the method launches a RayCastAll and collect all colliders on its way to a table named hitInfo.
     * The hitInfo table is parsed to determine if each collider is in front of the character or between the camera and the character.
     **/
    private void FixedUpdate()
    {
        Vector3 vPlayer = eyes.transform.position - transform.position;
        Vector3 vPlayerProjected = Vector3.Project(vPlayer, transform.forward);

        float characterDistance = vPlayerProjected.magnitude;
        float rayCastMaxRange = (vPlayerProjected).magnitude +5f;
        Debug.DrawRay(transform.position, transform.forward * rayCastMaxRange, Color.green);
        
        hitInfo = Physics.RaycastAll(transform.position, transform.forward, rayCastMaxRange);
        foreach (RaycastHit hit in hitInfo) {
            float objectDistance =(hit.point - transform.position).magnitude;
            Debug.DrawLine(transform.position, hit.point, Color.red);

            if (characterDistance < objectDistance)
            {
                SetBehaviorOfObjectsInFront(hit);
                return;
            }
            else
            {
                SetBehaviorOfObjectsBehind(hit);
            }
        }
    }
    
    /** SetBehaviorOfObjectsInFront Method 
     * @Params : RaycastHit
     * Set the behavior of Objects detected in front of the player. 
     * The method tries to get the type of the object (Mechanism, pickable etc...) if it has one and apply its behavior.
     **/
    private void SetBehaviorOfObjectsInFront (RaycastHit hit)
    {
        if (hit.transform.GetComponent<MechanismBase>())
        {
            GameObject objectInFrontOfPlayer = hit.transform.gameObject;
            MechanismBase mechanism = hit.transform.GetComponent<MechanismBase>();
            mechanism.DisplayTextOfMechanism();

            MakeGameObjectHighlighted scriptExisting = hit.transform.GetComponent<MakeGameObjectHighlighted>();
            if (scriptExisting == null)
                objectInFrontOfPlayer.AddComponent<MakeGameObjectHighlighted>();
            else
                scriptExisting.BeHighLighted();

            if (Input.GetKey(InputsProperties.activate))
                mechanism.ActivateMechanism();
        }
    }
    
    /** SetBehaviorOfObjectsBehind Method 
     * @Params : RaycastHit
     * Set the behavior of Objects detected behind the player. 
     * When an object is detected, all of its children is parsed.
     * The method add an instance of MakeGameObjectTransparent script on the gameObject and launch the method BeTransparent of that script.
     * This will change the transparency of all the gameobjects behind the character allowing a better visibility for the player.
     **/
    private void SetBehaviorOfObjectsBehind(RaycastHit hit)
    {
        Transform[] objects = hit.transform.GetComponentsInChildren<Transform>();
        foreach (Transform objectBehindPlayer in objects)
        {
            MakeGameObjectTransparent scriptExisting = objectBehindPlayer.GetComponent<MakeGameObjectTransparent>();
            if (scriptExisting == null)
                objectBehindPlayer.gameObject.AddComponent<MakeGameObjectTransparent>();
            else
                scriptExisting.BeTransparent();
        }
    }
}
