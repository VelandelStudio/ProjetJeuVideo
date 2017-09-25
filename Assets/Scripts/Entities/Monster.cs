using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the abstract Class of Monster
/// A monster has a target and can switch target    
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract class Monster : EntityLivingBase
{
    //Save the target of monster as a GameObject.
    private GameObject _target;

    /// <summary>
    /// The maximum distance of target before reset target
    /// </summary>
    [SerializeField] private double maxDistanceTarget;
    /// <summary>
    /// Initializes a new instance of the <see cref="Monster"/> class.
    /// </summary>
    public Monster()
    {
        _target = null;
    }

    /// <summary>
    /// Called when [trigger enter].
    /// </summary>
    /// <param name="other">The collider of another object.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only some collider can be a target.
        if (other.tag != "Player")
        {
            return;
        }

        Target = other.gameObject;
        OnTargetSelected();
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
        OnLoseTarget();
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

    /// <summary>
    /// Abstract method. 
    /// Called when [target selected].
    /// </summary>
    public abstract void OnTargetSelected();

    /// <summary>
    /// Abstract method.
    /// Called when [lose target].
    /// </summary>
    public abstract void OnLoseTarget();

    public double DistanceToTarget
    {
        get
        {
            var X = TargetTransform.position.x - this.transform.position.x;
            var Y = TargetTransform.position.y - this.transform.position.y;
            var Z = TargetTransform.position.z - this.transform.position.z;

            return Mathf.Sqrt((Mathf.Pow(X,2))+ (Mathf.Pow(Y, 2)) + (Mathf.Pow(Z, 2)));
        }
    }


    /// <summary>
    /// Updates this instance.
    /// </summary>
    private void Update()
    {
        if (DistanceToTarget > maxDistanceTarget)
        {
            ResetTarget();
        }
    }
}
