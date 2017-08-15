using UnityEngine;

/** Interractor Script
 * @Requires Camera
 * Get the Camera on Start and launch a RayCastAll from this Camera to the center of the screen.
 * The RayCastAll is a short distance ray made to represent if the player is close enough to something interractable.
 * Only use with the player fps cam is recommended.
 **/
[RequireComponent(typeof(Camera))] 
public class Interractor : MonoBehaviour {
    [SerializeField]
    private GameObject eyes;

    private Camera mainCamera;
    private RaycastHit[] hitInfo;

    /** Start Method 
     * Get the MainCamera of the game.
     **/
    private void Start() {
        mainCamera = GetComponent<Camera>();
    }

    /** FixedUpdate Method 
     * Get the distance between the camera and the player eyes then draw a DebugRay from the center of the camera to a position forward.
     * The maxDistance of the ray was fixed arbitrary to characterDistance*1.5f. In that way, if the players dezoom the camera, the max distance remains the same.
     * Afer that, the method launches a RayCastAll and collect all colliders on its way to a table named hitInfo.
     * The hitInfo table is parsed to determine if each collider is in front of the character or between the camera and the character.
     **/
    private void FixedUpdate() {
        Vector3 vPlayer = eyes.transform.position - transform.position;
        Vector3 vPlayerProjected = Vector3.Project(vPlayer, transform.forward);

        float characterDistance = vPlayerProjected.magnitude;
        float rayCastMaxRange = (vPlayerProjected * (1.5f)).magnitude;
        Debug.DrawRay(transform.position, vPlayerProjected * (1.5f), Color.green);
        
        hitInfo = Physics.RaycastAll(transform.position, transform.forward, rayCastMaxRange);
        foreach (RaycastHit hit in hitInfo) {
            float objectDistance =(hit.point - transform.position).magnitude;
            Debug.DrawLine(transform.position, hit.point, Color.red);

            if (characterDistance < objectDistance) {
                SetBehavioOfItemsInFront(hit);
                return;
            }
            else
                Debug.Log("Item " + hit.transform.name + " between player and camera");
            //SetItemsBehindOfPlayerBehavior -> Feature to develop in the future
        }
    }

    private void SetBehavioOfItemsInFront (RaycastHit hit) {
        if (hit.transform.GetComponent<MechanismBase>()) {
            MechanismBase mechanism = hit.transform.GetComponent<MechanismBase>();
            mechanism.DisplayTextOfMechanism();
            if (Input.GetButton("Fire2"))
                mechanism.ActivateMechanism();
        }
    }
}
