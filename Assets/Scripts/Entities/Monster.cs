using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>

/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Monster : EntityLivingBase
{
    private bool _isPlayerDetected = false;
    private Transform _targetTransform;
    private GameObject _target;

    private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            return;
        }

        Target = other.gameObject;
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
            return _targetTransform;
        }
        set
        {
            _targetTransform = value;
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
}
