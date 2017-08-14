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
     * Get the distance between the camera and the player eyes ther draw a DebugRay from the center of the camera to a position forward.
     * The maxDistance of the ray was fixed arbitrary to characterDistance+3. In that way, if the players dezoom the camera, the max distance remains the same.
     * Afer that, the method launches a RayCastAll and collect all colliders on its way to a table named hitInfo.
     * The hitInfo table is parsed to determine if each collider is in front of the character or between the camera and the character.
     **/
    private void FixedUpdate() {
        float characterDistance = (eyes.transform.position.normalized - transform.position.normalized).sqrMagnitude;
        Debug.DrawRay(transform.position, transform.forward * (characterDistance + 3.0f), Color.green);

        hitInfo = Physics.RaycastAll(mainCamera.transform.position, transform.forward, characterDistance + 3.0f);

        foreach (RaycastHit hit in hitInfo) {
            float objectDistance = (hit.transform.position.normalized - transform.position.normalized).sqrMagnitude;
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
