using System;
using System.Collections;
using UnityEngine;

/** PlayerController class
 * This class should always be attacjed to the player character 
 * OR on an entity controlled by the player (feature in the future).
 * This class detects every movements done by the player (real person) with his keyboard.
 * Then, it transmits the results of its calculations to the PlayerMotor.
 **/
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Animator _anim;
    private CharacterController _characterController;
    private bool _jumping;
    private bool _resetGravity;
    private float _gravity; 
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
        public float JumpSpeed = 6f;
        public float JumpTime = 0.25f;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        SetupAnimator();
    }

    private void Update()
    {
        ApplyGravity();
        Animate(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        _characterController.transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) ;

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    public void Animate(float forward, float strafe)
    {
        _anim.SetFloat(Animations.VerticalVelocityFloat, forward);
        _anim.SetFloat(Animations.HorizontalVelocityFloat, strafe);
        _anim.SetBool(Animations.GroundedBool, _characterController.isGrounded);
        _anim.SetBool(Animations.JumpBool, _jumping);
    }

    private void ApplyGravity()
    {
        Vector3 gravityVector = new Vector3();

        if (!_characterController.isGrounded)
        {
            if(!_resetGravity)
            {
                _gravity = Physics.ResetGravityValue;
                _resetGravity = true;
            }
            _gravity += Time.deltaTime + Physics.GravityModifier;
        }
        else
        {
            _gravity = Physics.BaseGravity;
            _resetGravity = false;
        }

        if(!_jumping)
        {
            gravityVector.y -= _gravity;
        }
        else
        {
            gravityVector.y = Movement.JumpSpeed;
        }
        _characterController.Move(gravityVector * Time.deltaTime);
    }

    public void Jump()
    {
        if(_jumping)
        {
            return;
        }

        if(_characterController.isGrounded)
        {
            _jumping = true;
            StartCoroutine(StopJump());
        }
    }

    private IEnumerator StopJump()
    {
        yield return new WaitForSeconds(Movement.JumpTime);
        _jumping = false;
    }

    private void SetupAnimator()
    {
        Animator[] animators = GetComponentsInChildren<Animator>();
        if(animators.Length > 0)
        {
            for (int i = 0; i < animators.Length; i++)
            {
                Animator anim = animators[i];
                Avatar av = anim.avatar;
                if(anim != _anim)
                {
                    _anim.avatar = av;
                    Destroy(anim);
                }
            }
        }
    }
}