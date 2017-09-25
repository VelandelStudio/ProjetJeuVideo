using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the abstract Class of Monster
/// A monster has a target and can switch target
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Monster : EntityLivingBase
{

    private GameObject _target;

    private void Start()
    {
       
    }

    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The collider of another object.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        Target = other.gameObject;
        GoOnRangeToAttack();
    }

    /// <summary>
    /// Gets or sets the target transform.
    /// </summary>
    /// <value>
    /// The target transform.
    /// </value>
    private Transform TargetTransform
    {
        get
        {
            return Target.transform;
        }

    }

    public bool IsPlayerDetected
    {
        get
        {
            return !(_target == null);
        }
    }
    /// <summary>
    /// Resets the target.
    /// There is no target.
    /// </summary>
    private void ResetTarget()
    {
        Target = null;
    }

    /// <summary>
    /// Gets or sets the target.
    /// </summary>
    /// <value>
    /// The target.
    /// </value>
    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
        }
    }

    public void GoOnRangeToAttack()
    {
        
    } 
}
