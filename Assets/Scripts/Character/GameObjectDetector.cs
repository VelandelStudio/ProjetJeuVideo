using UnityEngine;

/** GameObjectDetector Script
 * @Requires Camera
 * Get the Camera on Start and launch a RayCastAll from this Camera to the center of the screen.
 * The RayCastAll is a short distance ray made to represent if the player is close enough to something interractable.
 * Only use with the player fps cam is recommended.
 **/
[RequireComponent(typeof(Camera))] 
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
                Debug.Log("Item " + hit.transform.name + " between player and camera");
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
        if (hit.transform.GetComponent<IInterractableEntity>() != null)
        {
            GameObject objectInFrontOfPlayer = hit.transform.gameObject;
            IInterractableEntity interractable = hit.transform.GetComponent<IInterractableEntity>();
            interractable.DisplayTextOfInterractable();

            MakeGameObjectHighlighted scriptExisting = hit.transform.GetComponent<MakeGameObjectHighlighted>();
            if (scriptExisting == null)
                objectInFrontOfPlayer.AddComponent<MakeGameObjectHighlighted>();
            else
                scriptExisting.BeHighLighted();

            if (Input.GetKeyDown(InputsProperties.activate))
                interractable.ActivateInterractable();
        }
    }
    
    /** SetBehaviorOfObjectsBehind Method 
     * @Params : RaycastHit
     * Set the behavior of Objects detected behind the player. 
     * The method add an instance of MakeGameObjectTransparent script on the gameObject and launch the method BeTransparent of that script.
     * This will change the transparency of all the gameobjects behind the character allowing a better visibility for the player.
     **/
    private void SetBehaviorOfObjectsBehind(RaycastHit hit)
    {
        GameObject objectsBehindPlayer = hit.transform.gameObject;
        if (objectsBehindPlayer.tag == "Player")
            return;

        MakeGameObjectTransparent scriptExisting = objectsBehindPlayer.GetComponent<MakeGameObjectTransparent>();
        if (scriptExisting == null)
            objectsBehindPlayer.AddComponent<MakeGameObjectTransparent>();
        else
            scriptExisting.BeTransparent();
    }
}
