using UnityEngine;
/** CameraController, public class
 * The PlayerCamera is controlled by this script. It should always be attached to this camera, or on cameras that the player can control.
 * This script is directly linked to the player Mouse. It will set the camera position, rotation and zoom in function of the player action on the mouse.
 * Moreover, this script can be controlled by other script in order to freeze the camera. This will allow us to controll the layer camera sometimes. 
 * For exemple, the CursorStat controlls and freeze the camera = The camera won't move when the player is navigating in the menu.
 **/
public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform _target;					//The player transform.
    public bool CameraControlled;        //Boolean to enable/Disable when another script is controlling the camera.

    //Speed and sensitivity of the Camera.
    private float _xSpeed = 250.0f;
    private float _ySpeed = 120.0f;
    private float _sensitivity = 0.02f;

    //Clamp vertical values of rotation : the player can not see under his own foot.
    private float _xMinLimit = -10f;
    private float _xMaxLimit = 80f;

    //Clamp zoom values of the camera
    private float _distanceMin = 2.5f;
    private float _distance = 5.0f;
    private float _distanceMax = 6.0f;
    private float _zoomRate = 20f;

    public float X { get; private set; }
    public float Y { get; private set; }

    /** Start, private void
	 * This Method get the eulerAgnles of the transform Target (player).
	 * Then it sets the x and y values to the camera's one.
	 **/
    private void Start()
    {
        var angles = transform.eulerAngles;
        X = angles.y;
        Y = angles.x;
        CameraControlled = true;
    }

    /** LateUpdate, private void
	 * This method is called to set the new values of X and Y of the camera.
	 * It is called in a LateUpdate because the camera should always be ajusted after all other elements (As it is said on unity Documentation).
	 * We first test if the Camera is controlled by the player or by another script.
	 * If it is controlled by the player, we modify the input axis of the mouse by the speed and the sentivity and then apply a transform (position + rotation) and a zoom.
	 **/
    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (CameraControlled && !CursorBehaviour.CursorIsVisible)
        {
            Y -= Input.GetAxis("Mouse Y") * _ySpeed * _sensitivity;
            X += Input.GetAxis("Mouse X") * _xSpeed * _sensitivity;
        }

        HandleCameraZoom();
        HandleCameraTransform();
    }

    /** HandleCameraZoom, private void
	 * This method is called in every LateUpdate. It get the InputAxis ScrollWheel and modify the zoom value.
	 * Then this value is Clamped between distanceMin and distanceMax.
	 **/
    private void HandleCameraZoom()
    {
        _distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * _zoomRate * Mathf.Abs(_distance);
        _distance = _distance < _distanceMin ? _distanceMin : (_distance > _distanceMax ? _distanceMax : _distance);
    }

    /** HandleCameraTransform, private void
	 * This method is called in every LateUpdate. First at all, we clamp the angle value.
	 * Then this position and rotation of the camera are modified by the mouse Axis x and y.
	 **/
    private void HandleCameraTransform()
    {
        Y = ClampAngle(Y, _xMinLimit, _xMaxLimit);
        transform.rotation = Quaternion.Euler(Y, transform.parent.eulerAngles.y, 0);
        transform.position = transform.rotation * new Vector3(0.0f, 2.0f, -_distance) + _target.position;

        /*
        Y = ClampAngle(Y, _xMinLimit, _xMaxLimit);
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse Y"), Vector3.right);
        transform.position = transform.rotation * new Vector3(0.0f, 2.0f, -_distance) + _target.position;
        */
    }

    /** ClampAngle, private float
	 * This method is used to clamp the angle of the camera in order to  turn around the player on the horizontal axis.
	 **/
    private float ClampAngle(float angle, float min, float max)
    {
        angle = angle < -360 ? angle + 360 : (angle > 360 ? angle - 360 : angle);
        return Mathf.Clamp(angle, min, max);
    }

    /** ControlCamera, public void
	 * This method is used to clamp the angle of the camera in order to  turn around the player on the horizontal axis.
	 **/
    public void ControlCamera(float x, float y)
    {
        X = x;
        Y = y;
    }
}