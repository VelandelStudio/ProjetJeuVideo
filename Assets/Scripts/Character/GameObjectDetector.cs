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
    [SerializeField] private GameObject eyes;
    [SerializeField] private GameObject frontDetector;

    [SerializeField] private float rayCastMaxRange = 5f;

	private int layerMask;
	
	private void Start() {
		layerMask = LayerMask.NameToLayer("TriggerInterractableEntity");
	}
	
    /** FixedUpdate Method 
     * Get the distance between the camera and the player eyes then draw a DebugRay from the center of the camera to a position forward.
     * The maxDistance of the ray was fixed arbitrary to characterDistance*1.5f. In that way, if the players dezoom the camera, the max distance remains the same.
     * Afer that, the method launches a RayCastAll and collect all colliders on its way to a table named hitInfo.
     * The hitInfo table is parsed to determine if each collider is in front of the character or between the camera and the character.
     **/
	private void FixedUpdate()
	{
		Vector3 vPlayerProjected = Vector3.Project(frontDetector.transform.position - transform.position, transform.forward);
		float offSet = Vector3.Distance(eyes.transform.position, transform.position);
		Vector3 originPoint = transform.position+vPlayerProjected;
					
		Debug.DrawRay(originPoint, transform.forward * rayCastMaxRange, Color.green);
		Debug.DrawRay(transform.position, (eyes.transform.position - transform.position), Color.yellow);
		
		RaycastHit[] hitInFront;
        hitInFront = Physics.RaycastAll(originPoint, transform.forward, rayCastMaxRange);
        foreach (RaycastHit hitFront in hitInFront)
            if (isElligibleForDetection(hitFront))
            {
                SetBehaviorOfObjectsInFront(hitFront);
                break;
            }
				
		RaycastHit[] hitsBehind;
		hitsBehind = Physics.RaycastAll(transform.position, eyes.transform.position - transform.position,offSet);
        foreach (RaycastHit hit in hitsBehind)
            if(hit.transform.tag != "Player")
                SetBehaviorOfObjectsBehind(hit);
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
			
			if(objectInFrontOfPlayer.GetComponent<Renderer>() == null) 
				objectInFrontOfPlayer = objectInFrontOfPlayer.GetComponentsInChildren<Renderer>()[0].gameObject;
				
			MakeGameObjectHighlighted scriptExisting = objectInFrontOfPlayer.GetComponent<MakeGameObjectHighlighted>();
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
        Transform[] objectsBehindPlayer = hit.transform.gameObject.GetComponentsInChildren<Transform>();
		GameObject obj;
		foreach (Transform tr in objectsBehindPlayer) {
			obj = tr.gameObject;
			if(obj.GetComponent<Renderer>() == null)
				continue;
				
			MakeGameObjectTransparent scriptExisting = obj.GetComponent<MakeGameObjectTransparent>();
			
			if (scriptExisting == null)
				obj.AddComponent<MakeGameObjectTransparent>();
			else
				scriptExisting.BeTransparent();
		}
    }
	
	private bool isElligibleForDetection(RaycastHit hit) {
		return (!hit.collider.isTrigger || hit.collider.gameObject.layer == layerMask);
	}
}
