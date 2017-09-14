using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;
    Rigidbody rb;
    float distanceMin = 2.5f;
    float distance = 5.0f;
    float distanceMax = 10.0f;

    float xSpeed = 250.0f;
    float ySpeed = 120.0f;

    float yMinLimit = -10f;
    float yMaxLimit = 80f;

    float zoomRate = 20f;

    private float x;
    private float y;

    private void Start()
    {
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        rb = GetComponent<Rigidbody>();

        if (rb != null)
            rb.freezeRotation = true;
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        HandleCameraZoom();
        HandleCameraTransform();
    }

    private float ClampAngle(float angle, float min, float max)
    {
        angle = angle < -360 ? angle + 360 : (angle > 360 ? angle - 360 : angle) ;
        return Mathf.Clamp(angle, min, max);
    }

    private void HandleCameraZoom()
    {
        distance += -(Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime) * zoomRate * Mathf.Abs(distance);
        distance = distance < distanceMin ? distanceMin : (distance > distanceMax ? distanceMax : distance);
    }

    private void HandleCameraTransform()
    {
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        transform.rotation = Quaternion.Euler(y, x, 0);
        transform.position = transform.rotation * new Vector3(0.0f, 2.0f, -distance) + target.position;
    }
}
