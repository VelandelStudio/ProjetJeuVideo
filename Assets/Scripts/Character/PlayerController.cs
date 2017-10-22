using UnityEngine;

/** PlayerController class
 * This class should always be attacjed to the player character 
 * OR on an entity controlled by the player (feature in the future).
 * This class detects every movements done by the player (real person) with his keyboard.
 * Then, it transmits the results of its calculations to the PlayerMotor.
 **/
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PlayerController : MonoBehaviour {
    private Animator _anim;
    private Transform _camera;
    private float _sensitivity = 1.2f;
    private bool _isWalking = false;
    private Rigidbody _rb;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _camera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        Turning();
        Walking();
        Run();
    }

    private void Turning()
    {
        //_anim.SetFloat("Turn", Input.GetAxis("Mouse X"));
    }

    private void Walking()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _isWalking = !_isWalking;
            _anim.SetBool("Walk", _isWalking);
        }
    }
    private void Run()
    {
        _anim.SetFloat("Forward", Input.GetAxis("Vertical"));
    }
}