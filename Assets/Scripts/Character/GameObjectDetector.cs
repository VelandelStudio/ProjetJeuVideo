using UnityEngine;

/** GameObjectDetector Script
 * The GameObjectDetector class is used to detect object in front of and behind the player in his environnement.
 * This script has two main aims : Detect if items in front of the player are interractable and MakeTransparent items behind the player.
 * In order to shoot Raycast properly, we are using 2 empty GameObjects (backDetector and frontDetector) attached to the player.
 * These gameObjects are used as "origin" of both of the raycasts.
 **/
public class GameObjectDetector : MonoBehaviour
{
    [SerializeField] private GameObject backDetector;
    [SerializeField] private GameObject frontDetector;

    [SerializeField] private float rayCastMaxRange = 5f;

    private int layerMask;

    /** Start, private void method.
	 * The start Method is used the get the layerMask TriggerInterractableEntity as an integer.
	 * This layer is used for Interractable gameObjects launched by trigers only.
	 **/
    private void Start()
    {
        layerMask = LayerMask.NameToLayer("TriggerInterractableEntity");
    }

    /** FixedUpdate Method 
     * This method to fire both of the Rayasts.
	 *
	 * First of all, we launch the RayCast in front off the player.
	 * The Origin point of the raycast is the projected vector of the frontDetector + the distance between the player and the camera in the projected plan.
	 * Then, we shoot this raycast towards the camera transfmorm.forward direction. In this way, the player (irl person) can aim with the mouse.
	 * Please note that this Raycast is a RaycastAll, so we collect ALL of the gameObjects in front of the player. So we need to check if theses elements are elligible for a detection.
	 * in a foreach loop, we try to get if the element is an obstacle (break the loop) and if it is not, we check if the element can be highlighted (intterractable). If it his, we also break the loop.
	 * Please note that this method looks like a little bit touchy, but it is the only way to pass through triggers and hilight interractable triggers and non triggers. 
	 * The breaks are here to ensure that only the frist interractable detected is activable.
	 * 
	 * After that, we launch the behind the player, from the camera, to the backDetector.
	 * Then, we collect all of the GameObjects and make them Transparent if they are elligible for that.
     **/
    private void FixedUpdate()
    {
        Vector3 vPlayerProjected = Vector3.Project(frontDetector.transform.position - transform.position, transform.forward);
        Vector3 originPointFront = transform.position + vPlayerProjected;
        float offSet = Vector3.Distance(backDetector.transform.position, transform.position);

        Debug.DrawRay(originPointFront, transform.forward * rayCastMaxRange, Color.green);
        RaycastHit[] hitInFront;
        hitInFront = Physics.RaycastAll(originPointFront, transform.forward, rayCastMaxRange);
        foreach (RaycastHit hitFront in hitInFront)
        {
            if (ObstacleDetectedInFront(hitFront))
                break;

            if (IsElligibleForHighlight(hitFront))
            {
                SetBehaviorOfObjectsInFront(hitFront);
                break;
            }
        }

        Debug.DrawRay(transform.position, (backDetector.transform.position - transform.position), Color.yellow);
        RaycastHit[] hitsBehind;
        hitsBehind = Physics.RaycastAll(transform.position, backDetector.transform.position - transform.position, offSet);
        foreach (RaycastHit hit in hitsBehind)
            if (hit.transform.tag != "Player" && IsElligibleForTransparency(hit))
                SetBehaviorOfObjectsBehind(hit);
    }

    /** SetBehaviorOfObjectsInFront Method 
     * @Params : RaycastHit
     * Set the behavior of Objects detected in front of the player. 
     * The method tries to get the type of the object (Mechanism, pickable etc...) if it has one and apply its behavior.
     **/
    private void SetBehaviorOfObjectsInFront(RaycastHit hit)
    {
        if (hit.transform.GetComponent<IInterractableEntity>() != null)
        {
            GameObject objectInFrontOfPlayer = hit.transform.gameObject;
            IInterractableEntity interractable = hit.transform.GetComponent<IInterractableEntity>();
            interractable.DisplayTextOfInterractable();

            if (objectInFrontOfPlayer.GetComponent<Renderer>() == null)
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
        foreach (Transform tr in objectsBehindPlayer)
        {
            obj = tr.gameObject;
            if (obj.GetComponent<Renderer>() == null)
                continue;

            MakeGameObjectTransparent scriptExisting = obj.GetComponent<MakeGameObjectTransparent>();

            if (scriptExisting == null)
                obj.AddComponent<MakeGameObjectTransparent>();
            else
                scriptExisting.BeTransparent();
        }
    }

    /** ObstacleDetectedInFront private bool Method 
     * @Params : RaycastHit
     * This method is used to detect if the player is aiming at an obstacle. 
	 * This method should be used in order to not detect interractable if they are hidden by a wall or an obstacle.
     **/
    private bool ObstacleDetectedInFront(RaycastHit hit)
    {
        return (!hit.collider.isTrigger && hit.transform.GetComponent<IInterractableEntity>() == null && hit.collider.gameObject.layer != layerMask);
    }

    /** IsElligibleForHighlight private bool Method 
     * @Params : RaycastHit
     * This method is used to detect if the player is aiming at an interractable. 
	 * It ensures a good way to detect Colliders AND interractable Triggers with the layerMask.
     **/
    private bool IsElligibleForHighlight(RaycastHit hit)
    {
        return (!hit.collider.isTrigger || hit.collider.gameObject.layer == layerMask);
    }

    /** IsElligibleForTransparency private bool Method 
     * @Params : RaycastHit
     * This method is used to detect if a gameobject, detected by a raycast, is Elligible For Transparency.
	 * It ensures a good way to detect only colliders and not triggers inettractable or not.
     **/
    private bool IsElligibleForTransparency(RaycastHit hit)
    {
        return !(hit.collider.isTrigger && hit.transform.GetComponent<IInterractableEntity>() != null && hit.collider.gameObject.layer != layerMask);
    }
}