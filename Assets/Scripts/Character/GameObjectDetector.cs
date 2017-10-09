using UnityEngine;

/** GameObjectDetector Script
 * The GameObjectDetector class is used to detect object in front of and behind the player in his environnement.
 * This script has two main aims : Detect if items in front of the player are interractable and MakeTransparent items behind the player.
 * In order to shoot Raycast properly, we are using 2 empty GameObjects (backDetector and frontDetector) attached to the player.
 * These gameObjects are used as "origin" of all of the raycasts.
 **/
public class GameObjectDetector : MonoBehaviour
{
    [SerializeField] private GameObject _backDetector;
    [SerializeField] private GameObject _frontDetector;

    [SerializeField] private float _rayCastMaxRange = 5f;

    public Vector3 OriginPoint;
    private int _layerMask;
    private Transform _cameraTransform;
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
	 * First of all, we launch the RayCast in front off the player.
	 * The Origin point of the raycast is the projected vector of the frontDetector + the distance between the player and the camera in the projected plan.
	 * Then, we shoot this raycast towards the camera transfmorm.forward direction. In this way, the player (irl person) can aim with the mouse.
	 * Please note that there are two Raycasts : one to detect solid interractables and one to detect triggers interractable.
     * When an interractable is detected, we check is he is elligible for the detection (in order to not highlight a trigger instead of a collider for example).
	 * 
	 * After that, we launch the behind the player, from the camera, to the backDetector.
	 * Then, we collect all of the GameObjects and make them Transparent if they are elligible for that.
     **/
    private void FixedUpdate()
    {
        Vector3 vPlayerProjected = Vector3.Project(_frontDetector.transform.position - _cameraTransform.position, _cameraTransform.forward);
        Vector3 originPointFront = _cameraTransform.position + vPlayerProjected;
        float offSet = Vector3.Distance(_backDetector.transform.position, _cameraTransform.position);

        Debug.DrawRay(originPointFront, _cameraTransform.forward * _rayCastMaxRange, Color.green);

        RaycastHit hitInFrontCollider;
        OriginPoint = originPointFront;
        bool interractableCollider = Physics.Raycast(originPointFront, _cameraTransform.forward, out hitInFrontCollider, _rayCastMaxRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (interractableCollider && IsElligibleForHighlight(hitInFrontCollider))
        {
            SetBehaviorOfObjectsInFront(hitInFrontCollider);
        }

        RaycastHit hitInFrontTrigger;
        bool interractableTrigger = Physics.Raycast(originPointFront, _cameraTransform.forward, out hitInFrontTrigger, _rayCastMaxRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);
        if (interractableTrigger && IsElligibleForHighlight(hitInFrontTrigger))
        {
            SetBehaviorOfObjectsInFront(hitInFrontTrigger);
        }
        Debug.DrawRay(_cameraTransform.position, (_backDetector.transform.position - _cameraTransform.position), Color.yellow);
        RaycastHit[] hitsBehind;
        hitsBehind = Physics.RaycastAll(_cameraTransform.position, _backDetector.transform.position - _cameraTransform.position, offSet);
        foreach (RaycastHit hit in hitsBehind)
        {
            if (hit.transform.tag != "Player" && IsElligibleForTransparency(hit))
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
    private void SetBehaviorOfObjectsInFront(RaycastHit hit)
    {
        if (hit.transform.GetComponent<IInterractableEntity>() != null)
        {
            GameObject objectInFrontOfPlayer = hit.transform.gameObject;
            IInterractableEntity interractable = hit.transform.GetComponent<IInterractableEntity>();
            interractable.DisplayTextOfInterractable();
            Debug.Log(hit.collider.name);

            MakeGameObjectHighlighted scriptExisting = objectInFrontOfPlayer.GetComponent<MakeGameObjectHighlighted>();
            if (scriptExisting == null)
            {
                objectInFrontOfPlayer.AddComponent<MakeGameObjectHighlighted>();
            }
            else
            {
                scriptExisting.BeHighLighted();
            }

            if (Input.GetKeyDown(InputsProperties.activate))
            {
                interractable.ActivateInterractable();
            }
        }
    }

    /** SetBehaviorOfObjectsBehind Method 
     * @Params : RaycastHit
     * Set the behavior of Objects detected behind the player. 
     * The method add an instance of MakeGameObjectTransparent script on the gameObject and launch the method BeTransparent of that script.
     * This will change the transparency of all the gameobjects behind the character allowing a better visibility for the player.
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

            MakeGameObjectTransparent scriptExisting = obj.GetComponent<MakeGameObjectTransparent>();
            if (scriptExisting == null)
            {
                obj.AddComponent<MakeGameObjectTransparent>();
            }
            else
            {
                scriptExisting.BeTransparent();
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
        return (!hit.collider.isTrigger || hit.collider.gameObject.layer == _layerMask);
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