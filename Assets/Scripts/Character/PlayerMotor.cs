using UnityEngine;
using System.Collections;

/** PlayerMotor Class
 * This class is always attached to a Player.
 * This class handles the player movements and rotations inputed by the player (real person).
 * It must have a collider and a RigidBody in order to move properly.
 **/
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMotor : MonoBehaviour {

    [SerializeField] private Camera playerCamera;

    private Vector3 velocity = Vector3.zero;
    private Vector3 horizontalRotation = Vector3.zero;

    private Rigidbody rb;
	private bool unableToMove = false;
	
	/** Start, private void method
	 * This Start method allows to get the RigidBody of our Player
	 **/
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	/** FixedUpdate, private void method
	 * Every actions of this class are physics commands. That's why we are using here a FixedUpdate instead of an simple Update.
	 * When Launched (every 0.2s by MonoBehaviour class), it launches the moethods PerformMovementOfPlayer and PerformRotationOfPlayer
	 **/
    private void FixedUpdate()
    {
        PerformMovementOfPlayer();
        PerformRotationOfPlayer();
    }
	
	/** PerformMovementOfPlayer, private void method
	 * This method is used in two steps. 
     * First at all, it checks if the player is unableToMove (stun, dead etc...).
	 * If he is not able to move, the method returns;
	 * Else, it launches a RigidBody.MovePosition().
	 * The new position of the RigidBody is AncientPosition + Velocity * fixedDeltaTime,
	 * where the Velocity whould be set by the MovePlayer Method.
 	 **/
    private void PerformMovementOfPlayer()
    {
        if (unableToMove)
            return;

        if (velocity != Vector3.zero)
                rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
	
	/** MovePlayer, Public void method
	 * This method should always be called by other classes when they want to make the player move.
	 * This method just passes a new velocity to the player.
	 * This velocity will be applied in the PerformMovementOfPlayer method.
 	 **/
    public void MovePlayer(Vector3 velocity)
    {
        this.velocity = velocity;
    }
	
	/** PerformRotationOfPlayer, private void method
	 * This method ensure that the Y rotation of a the player is always equal to the Y rotation of the Camera
 	 **/
    private void PerformRotationOfPlayer()
    {
        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        horizontalRotation = new Vector3(0, cameraRotation.y,0);
        transform.rotation = Quaternion.Euler(horizontalRotation);
    }
	
	/** PerformRotationOfPlayer, Public void method
	 * This public method should always be called by other classes when they want to make the player able or unable to move.
	 **/
    public void SetUnableToMove(bool b) {
		unableToMove = b;
	}
}
