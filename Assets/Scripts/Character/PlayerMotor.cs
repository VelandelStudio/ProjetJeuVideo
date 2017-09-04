using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class PlayerMotor : MonoBehaviour {

    [SerializeField]
    private Camera playerCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 horizontalRotation = Vector3.zero;
    private Vector3 verticalRotation = Vector3.zero;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;
	private bool unableToMove = false;
	
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        PerformMovementOfPlayer();
        PerformRotationOfPlayer();
    }

    private void PerformMovementOfPlayer()
    {
        if (unableToMove)
            return;

        if (velocity != Vector3.zero)
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }

    public void MovePlayer(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    private void PerformRotationOfPlayer()
    {
        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        horizontalRotation = new Vector3(0, cameraRotation.y,0);
        transform.rotation = Quaternion.Euler(horizontalRotation);
    }

    public void SetUnableToMove(bool b) {
		unableToMove = b;
	}
}
