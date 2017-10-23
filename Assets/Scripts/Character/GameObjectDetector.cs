using UnityEngine;

/** GameObjectDetector Script
 * The GameObjectDetector class is used to detect object in front of and behind the player in his environnement.
 * This script has two main aims : Detect if items in front of the player are interractable and MakeTransparent items behind the player.
 * These gameObjects are used as "origin" of all of the raycasts.
 **/
public class GameObjectDetector : MonoBehaviour
{
    [SerializeField] private float _rayCastMaxRange;
    private int _layerMask;
    private Transform _cameraTransform;
    private Ray _cameraAim;

    /** Start, private void method.
	 * The start Method is used the get the layerMask TriggerInterractableEntity as an integer.
	 * This layer is used for Interractable gameObjects launched by trigers only.
	 **/
    private void Start()
    {
        _layerMask = LayerMask.NameToLayer("TriggerInterractableEntity");
        _cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    /** FixedUpdate Method 
     * This method to fire all of the Raycasts.
	 *
	 * First of all, we launch the RayCastAll from the camera to the camera.transform.forward and we collect every gameObjects.
	 * After that, we parse all of the gameObjects detected and launch a RayCast from the player to this GameObject.
	 * With this Raycast we are able to know if a gameObjetc is in front of the Player or behind the player.
	 * After that we treat each case sepratly and apply Transparency on all GameObjects behind the player and highlight the first interractable encountered in front of the player.
     **/
    private void FixedUpdate()
    {
        _cameraAim = _cameraTransform.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit[] rayCastHits = Physics.RaycastAll(_cameraAim, 20f);
        foreach (RaycastHit cameraHit in rayCastHits)
        {
            Vector3 cameraHitPoint = cameraHit.point;
            RaycastHit playerHit;
            Physics.Raycast(transform.position + new Vector3(0f, 1.8f, 0f), cameraHitPoint - transform.position - new Vector3(0f, 1.8f, 0f), out playerHit, 20f);
            float playerDistance = playerHit.distance;
            Vector3 distanceToTarget = (playerHit.point - transform.position).normalized;

            if (Vector3.Dot(distanceToTarget, transform.forward) > 0)
            {
                if (playerDistance <= _rayCastMaxRange && playerHit.collider == cameraHit.collider && IsElligibleForHighlight(playerHit))
                {
                    Debug.DrawLine(_cameraAim.origin, cameraHit.point, Color.green);
                    Debug.DrawRay(transform.position + new Vector3(0f, 1.8f, 0f), cameraHitPoint - transform.position - new Vector3(0f, 1.8f, 0f), Color.red);
                    SetBehaviorOfObjectsInFront(playerHit);
                }
            }
            else
            {
                if (IsElligibleForTransparency(cameraHit))
                {
                    SetBehaviorOfObjectsBehind(cameraHit);
                }
            }
        }
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
            HighlightStatus scriptExisting = objectInFrontOfPlayer.GetComponentInChildren<HighlightStatus>();
            if (scriptExisting == null)
            {
                GameObject obj = (GameObject)Resources.Load("Status/Utils/HighlightStatus", typeof(GameObject));
                Instantiate(obj, objectInFrontOfPlayer.transform.position, objectInFrontOfPlayer.transform.rotation, objectInFrontOfPlayer.transform);
            }
            else
            {
                scriptExisting.ResetStatus();
            }

            if (Input.GetKeyDown(InputsProperties.Activate) && !GetComponent<CursorBehaviour>().CursorIsVisible)
            {
                interractable.ActivateInterractable();
            }
        }
    }

    /** SetBehaviorOfObjectsBehind Method 
     * @Params : RaycastHit
     * Set the behavior of Objects detected behind the player. 
     * The method add an instance of TransparentStatus script on the gameObject.
	 * The method will check for all the children GameObjects of the detected one. It will next test if the children is INSIDE the parent.
     **/
    private void SetBehaviorOfObjectsBehind(RaycastHit hit)
    {
        Transform[] objectsBehindPlayer = hit.transform.gameObject.GetComponentsInChildren<Transform>();
        GameObject obj;
        foreach (Transform tr in objectsBehindPlayer)
        {
            obj = tr.gameObject;
            if (obj.GetComponent<Renderer>() == null || !hit.collider.bounds.Intersects(obj.GetComponent<Renderer>().bounds))
            {
                continue;
            }

            TransparentStatus scriptExisting;
            if (obj.transform.childCount >= 1)
            {
                scriptExisting = obj.transform.GetChild(obj.transform.childCount - 1).GetComponent<TransparentStatus>();
            }
            else
            {
                scriptExisting = obj.GetComponentInChildren<TransparentStatus>();
            }

            if (scriptExisting == null)
            {
                GameObject instance = (GameObject)Resources.Load("Status/Utils/TransparentStatus", typeof(GameObject));
                Instantiate(instance, tr.position, tr.rotation, tr);
            }
            else
            {
                scriptExisting.ResetStatus();
            }
        }
    }

    /** IsElligibleForHighlight private bool Method 
     * @Params : RaycastHit
     * This method is used to detect if the player is aiming at an interractable. 
	 * It ensures a good way to detect Colliders AND interractable Triggers with the layerMask.
     **/
    private bool IsElligibleForHighlight(RaycastHit hit)
    {
        return ((!hit.collider.isTrigger || hit.collider.gameObject.layer == _layerMask) && hit.transform.GetComponent<IInterractableEntity>() != null);
    }

    /** IsElligibleForTransparency private bool Method 
     * @Params : RaycastHit
     * This method is used to detect if a gameobject, detected by a raycast, is Elligible For Transparency.
	 * It ensures a good way to detect only colliders and not triggers inettractable or not.
     **/
    private bool IsElligibleForTransparency(RaycastHit hit)
    {
        return !(hit.collider.isTrigger && hit.transform.GetComponent<IInterractableEntity>() != null && hit.collider.gameObject.layer != _layerMask);
    }
}