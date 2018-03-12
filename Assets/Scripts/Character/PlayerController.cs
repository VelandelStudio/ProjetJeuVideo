using System;
using System.Collections;
using UnityEngine;

/** PlayerController class
 * This class should always be attached to the player character 
 * OR on an entity controlled by the player (feature in the future).
 * This class detects every movements done by the player (real person) with his keyboard.
 * Then, it transmits the results of its calculations to the CharacterController.
 **/
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Animator _anim;
    private CharacterController _characterController;
    public bool _isFighting;
    private bool _jumping;
    private bool _resetGravity;
    private float _gravity;
    private Vector3 _gravityVector = new Vector3();
    private Vector3 _moveDirection = new Vector3();

    private float _lastHorizontalInput;
    private float _lastVerticalInput;

    private Characteristics _characteristics;
    [SerializeField] public AnimationSettings Animations;
    [SerializeField] public PhysicsSettings Physics;
    [SerializeField] public MovementSettings Movement;

    [System.Serializable]
    public class AnimationSettings
    {
        public string VerticalVelocityFloat = "Forward";
        public string HorizontalVelocityFloat = "Strafe";
        public string GroundedBool = "IsGrounded";
        public string JumpBool = "IsJumping";
    }

    [System.Serializable]
    public class PhysicsSettings
    {
        public float GravityModifier = 9.81f;
        public float BaseGravity = 50f;
        public float ResetGravityValue = 1.2f;
    }

    [System.Serializable]
    public class MovementSettings
    {
        public float MovementSpeed = 5f;
        public float JumpSpeed = 3f;
        public float JumpTime = 0.5f;
        public float MoveSpeedFactor = 1.0f;
    }

    /** Start : private void method
	 * Used to get every component of the Champion.
	 * Also used to call the SetupAnimator method.
	**/
    private void Start()
    {
        Camera.main.GetComponent<CameraController>().AttributChampionToFollow(transform);
        _anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _characteristics = GetComponent<Characteristics>();
        SetupAnimator();
    }

    /** Update : private void method
	 * At every frame, wel call the ApplyGravity method to make the sure that the player is falling or touching the ground.
	 * Then we transmit all of the inputs to the Animate method.
	 * After that we detect if the player is rotating and apply a rotation based on the mouse X movement.
	 * To finish, we launch the Jump method if the JumpInput is detected
	**/
    private void Update()
    {
        ApplyGravity();
        ApplyMovemement();
        if (!CursorBehaviour.CursorIsVisible)
        {
            Animate(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

            _characterController.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up);

            if (Input.GetKeyDown(InputsProperties.Jump))
            {
                Jump();
            }
            _lastHorizontalInput = Input.GetAxis("Horizontal");
            _lastVerticalInput = Input.GetAxis("Vertical");
        }
        else
        {
            _lastHorizontalInput = Mathf.Lerp(_lastHorizontalInput, 0, 0.1f);
            _lastVerticalInput = Mathf.Lerp(_lastVerticalInput, 0, 0.1f);
            Animate(_lastVerticalInput, _lastHorizontalInput);
        }

      /*  if (_isFighting)
        {
            _anim.SetBool("IsFighting", true);
        }
        else
        {
            _anim.SetBool("IsFighting", false);
        }
        */
    }

    /** Update : public void method
	 * Called at every frame by the Update Method.
	 * Sets the different value to the Animator to make sure that animations are correctly displayed in function of inputs.
	**/
    public void Animate(float forward, float strafe)
    {
        _anim.SetFloat(Animations.VerticalVelocityFloat, forward);
        _anim.SetFloat(Animations.HorizontalVelocityFloat, strafe);
        //_anim.SetBool(Animations.GroundedBool, _characterController.isGrounded);
        //_anim.SetBool(Animations.JumpBool, _jumping);
    }

    /** ApplyGravity : private void method
	 * Called at every frame by the Update Method.
	 * We apply gravity in two ways. 
	 * First of all, we apply it in a certain way when the player is grounded or not.
	 * When he is falling, the gravity is progressively griding (in that way, the component move faster when the fall is long).
	 * Then, we update gravity if the player is Jumping (opposite gravity) or Falling after Jump.
	 * After that, we Move our player depending on the differents keyboard inputs.
	**/
    private void ApplyGravity()
    {
        _gravityVector = Vector3.zero;

        if (!_characterController.isGrounded)
        {
            if (!_resetGravity)
            {
                _gravity = Physics.ResetGravityValue;
                _resetGravity = true;
            }
            _gravity += Time.deltaTime * Physics.GravityModifier;
        }
        else
        {
            _gravity = Physics.BaseGravity;
            _resetGravity = false;
        }

        if (!_jumping)
        {
            _gravityVector.y -= _gravity;
        }
        else
        {
            _gravityVector.y = Movement.JumpSpeed;
        }
    }

    /** ApplyMovemement : private void method
	 * Applys all the movements to the player. 
     * In this method we threat two cases separately that depends if the player has his cursor visible or no not.
	**/
    private void ApplyMovemement()
    {

        if (_characteristics)
        {
            Movement.MoveSpeedFactor = _characteristics.MovementSpeedFactor;
        }

        if (CursorBehaviour.CursorIsVisible)
        {
            _moveDirection = Vector3.zero;
            _moveDirection.y = _gravityVector.y;
            _characterController.Move(_moveDirection * Time.deltaTime);
        }
        else
        {
            if (Input.GetAxis("Vertical") < 0)
            {
                _moveDirection = (Movement.MovementSpeed * Movement.MoveSpeedFactor) / 4 * transform.TransformDirection(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            }
            else
            {
                _moveDirection = transform.TransformDirection(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            }
            _moveDirection.Normalize();

            _moveDirection.y = _gravityVector.y;
           //_characterController.Move((Movement.MovementSpeed * Movement.MoveSpeedFactor) * _moveDirection * Time.deltaTime);
        }
    }

    /** Jump : public void method
	 * Launch the Jump by setting a bool from false to true and launch the coroutine StopJump.
	**/
    public void Jump()
    {
        if (_jumping)
        {
            return;
        }

        if (_characterController.isGrounded)
        {
            _jumping = true;
            StartCoroutine(StopJump());
        }
    }

    /** StopJump : private IEnumerator method
	 * Launched by the Jump method. 
	 * When launched, it tells to the game "I am currently Jumping, don't apply gravity on me" during the JumpTime.
	**/
    private IEnumerator StopJump()
    {
        yield return new WaitForSeconds(Movement.JumpTime);
        _jumping = false;
    }

    /** SetupAnimator : private void method
	 * Tricky way to get the animator from children at the begining of the game.
	 * With this method, we can easily change the PlayerModel.
	 * This will be uselful if we can play different races with different bodies. 
	 * We will just need to change the model in the children but the script remains the same.
	**/
    private void SetupAnimator()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        if (animators.Length > 0)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                Animator anim = animators[i];
                Avatar av = anim.avatar;
                if (anim != _anim)
                {
                    _anim.avatar = av;
                    Destroy(anim);
                }
            }
        }
    }
}