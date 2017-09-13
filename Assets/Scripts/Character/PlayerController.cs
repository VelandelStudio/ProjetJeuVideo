using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]

public class PlayerController : MonoBehaviour {

    [Header("Movement Options")]
    [SerializeField]
    private float moveSpeed = 6f;
    [SerializeField]

    private PlayerMotor playerMotor;

    void Start () {
        playerMotor = GetComponent<PlayerMotor>();
    }

    void Update () {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        Vector3 horizontalMovement = Input.GetAxis("Horizontal") * transform.right;
        Vector3 verticalMovement = Input.GetAxis("Vertical") * transform.forward;
        Vector3 velocity = (horizontalMovement + verticalMovement).normalized * moveSpeed;

        playerMotor.MovePlayer(velocity);
    }
}