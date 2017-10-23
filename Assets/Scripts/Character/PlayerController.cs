using UnityEngine;

/** PlayerController class
 * This class should always be attacjed to the player character 
 * OR on an entity controlled by the player (feature in the future).
 * This class detects every movements done by the player (real person) with his keyboard.
 * Then, it transmits the results of its calculations to the PlayerMotor.
 **/
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 6f;

    private Vector3 _horizontalMovement;
    private Vector3 _verticalMovement;
    private PlayerMotor _playerMotor;
    private Vector3 _velocity;

    /** Start, private void method
	 * The Start method is used to get the PlayerMotor of the Character. 
	 * This class has a [RequireComponent(typeof(PlayerMotor))] so the PlayerMotor can not be null.
	 **/
    private void Start()
    {
        _playerMotor = GetComponent<PlayerMotor>();
    }

    /** Update, private void method
	 * The Update method is used to aggregate all of the private method associated to the player movement.
	 * At this moment, only CalculateMovement is launched
	 **/
    private void Update()
    {
        if (GetComponent<CursorBehaviour>().CursorIsVisible)
        {
            _velocity = Vector3.zero;
            _playerMotor.MovePlayer(_velocity);
        }
        else
        {
            CalculateMovement();
        }
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
        _horizontalMovement = HorizontalAxisCalculator() * transform.right;
        _verticalMovement = VerticalAxisCalculator() * transform.forward;

        _velocity = (_horizontalMovement + _verticalMovement).normalized * _moveSpeed;

        _playerMotor.MovePlayer(_velocity);
    }

    /** HorizontalAxisCalculator, private float method
	 * This method is used to get the HorizontalInputs key from the InputsProperties.
	 * Then we return a value that will be multiplied by transform.right in the CalculateMovement movement.
	 * If no keys are detected, we return 0 in order to set the velocity of the player to 0.
	 **/
    private float HorizontalAxisCalculator()
    {
        if (Input.GetKey(InputsProperties.StrafeLeft))
        {
            return -1;
        }

        if (Input.GetKey(InputsProperties.StrafeRight))
        {
            return 1;
        }

        return 0;
    }

    /** VerticalAxisCalculator, private float method
	 * This method is used to get the VerticalInputs key from the InputsProperties.
	 * Then we return a value that will be multiplied by transform.forward in the CalculateMovement movement.
	 * If no keys are detected, we return 0 in order to set the velocity of the player to 0.
	 **/
    private float VerticalAxisCalculator()
    {
        if (Input.GetKey(InputsProperties.MoveBackward))
        {
            return -1;
        }

        if (Input.GetKey(InputsProperties.MoveForward))
        {
            return 1;
        }

        return 0;
    }
}