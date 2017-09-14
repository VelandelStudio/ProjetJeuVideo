using UnityEngine;

/** PlayerController class
 * This class should always be attacjed to the player character 
 * OR on an entity controlled by the player (feature in the future).
 * This class detects every movements done by the player (real person) with his keyboard.
 * Then, it transmits the results of its calculations to the PlayerMotor.
 **/
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField] private float moveSpeed = 6f;
	
    private Vector3 horizontalMovement;
	private Vector3 verticalMovement;
	private PlayerMotor playerMotor;
	private Vector3 velocity;
	
	/** Start, private void method
	 * The Start method is used to get the PlayerMotor of the Character. 
	 * This class has a [RequireComponent(typeof(PlayerMotor))] so the PlayerMotor can not be null.
	 **/
    private void Start () {
        playerMotor = GetComponent<PlayerMotor>();
    }
	
	/** Update, private void method
	 * The Update method is used to aggregate all of the private method associated to the player movement.
	 * At this moment, only CalculateMovement is launched
	 **/
    private void Update () {
        CalculateMovement();
    }
	
	/** CalculateMovement, private void method
	 * The CalculateMovement is used in two ways. First at all, it is getting the horizontalMovement and the verticalMovement.
	 * In order to the that, it is watching both of the input.Axis (Horizontal and Vertical).
	 * Then, it calculates the velocity of the player, which is the normal vector of horizontalMovement and verticalMovement.
	 * In order the modify, in the future, the speed of our player, the velocity is multiplied by a variable movespeed.
	 * After that, the method passes the velocity value to the MovePlayer method in the PlayerMotor.
	 **/
    private void CalculateMovement()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * transform.right;
        verticalMovement = Input.GetAxis("Vertical") * transform.forward;
        velocity = (horizontalMovement + verticalMovement).normalized * moveSpeed;

        playerMotor.MovePlayer(velocity);
    }
}