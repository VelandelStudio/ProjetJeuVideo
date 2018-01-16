using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** WindPushStatus, public class
 * @extends : StatusBase
 * This Status aims to apply a vertical force on targets, which acts as a little stun.
 **/
public class WindPushStatus : StatusBase
{

    /** OnStatusApplied public override void
	 * The Status gets the rigidBody of its parent. We ensure that the parent has a rigidbody that uses gravity.
	 * Then we add a vertical force (Up).
	 **/
    public override void OnStatusApplied()
    {
        Rigidbody rb = GetComponentInParent<Rigidbody>();
        if (rb == null)
        {
            rb = transform.gameObject.AddComponent<Rigidbody>();
            Debug.Log("A Rigidbody was added to " + transform.parent.gameObject.name + " by " + Name + ". Please ensure that the absence of a rigidbody is not an issue.");
        }

        if (!rb.useGravity)
        {
            rb.useGravity = true;
            Debug.Log("A Rigidbody on " + transform.parent.gameObject.name + " associated with " + Name + " has no useGravity. Please ensure that it is not an issue.");
        }

        rb.AddForce(Vector3.up * 200);
    }

    public override void StatusTickBehaviour() { }
}