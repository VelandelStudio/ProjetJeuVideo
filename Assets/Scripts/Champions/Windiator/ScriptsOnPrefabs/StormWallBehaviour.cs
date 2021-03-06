﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** StormWallBehaviour, public class
 * This script should always be associated with the StormWall prefab which is one of the prefabs created by the Windiator Champion
 * The main object of the screen is to get the associated spell from the parent and contact it when something passes through the wall.
 * Please note that the prefab has a collider trigger.
 **/
public class StormWallBehaviour : MonoBehaviour
{

    private StormWallSpell _parentSpell;
    private BoxCollider _collider;
    private bool _wallMoving = true;
    private Vector3 origin;
    private float _maxRange;
    private float _speedWall = 20f;

    /** Start private void method
	 * The start method is used to get the StormWallSpell associated to this wall.
	 * Then we detach the gameObjet of its parent.
	 **/
    private void Start()
    {
        _parentSpell = GetComponentInParent<StormWallSpell>();
        _collider = GetComponent<BoxCollider>();
        origin = transform.position;
        transform.parent = null;
        transform.position += new Vector3(0, transform.lossyScale.y / 2, 0);

        if (_parentSpell != null)
        {
            _maxRange = float.Parse(_parentSpell.OtherValues[0]);
        }

        Destroy(gameObject, float.Parse(_parentSpell.OtherValues[1]));
        //Invoke("DestroyItself", float.Parse(_parentSpell.OtherValues[1]));
    }

    /// <summary>
    /// Private Update method
    /// Check if the wall is moving to stop it until it reach the max range
    /// Call some methods to stay on the ground and modidy its speed
    /// </summary>
    private void Update()
    {
        if (_wallMoving)
        {
            ModifySpeed();
            FitFloor();

            transform.Translate(Vector3.forward * Time.deltaTime * _speedWall);
            if (Vector3.Distance(transform.position, origin) >= _maxRange)
            {
                _wallMoving = false;
            }
        }
    }

    /** OnTriggerEnter private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerEnter(Collider other)
    {
        _parentSpell.ApplyEffect(other);
    }


    /** OnTriggerStay private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerStay(Collider other)
    {
        _parentSpell.ApplyEffect(other);
    }

    /** OnTriggerStay private void method
	 * @Param : Collider
	 * This method is thanks to the collider trigger.
	 * When something stays inside the wall, whatever it is, we launch the ApplyEffect method of the associated StormWallSpell.
	 **/
    private void OnTriggerExit(Collider other)
    {
        _parentSpell.ApplyEffect(other);
    }

    /*
    /// <summary>
    /// DestroyItself method
    /// Include inside the start with Invoke to destroy the wall after few seconds
    /// </summary>
    private void DestroyItself()
    {
        Destroy(gameObject);
    }
    */

    /// <summary>
    /// ModifySpeed method
    /// Ticky method called into update to slow down the wall speed.
    /// </summary>
    private void ModifySpeed()
    {
        if (Vector3.Distance(transform.position, origin) >= ((66f / 100f) * _maxRange))
        {
            _speedWall /= 2;
        }
    }

    /// <summary>
    /// FitFloor method
    /// Call into the update method to ensure that the wall fit correctly with the "Floor" gameObject.
    /// </summary>
    private void FitFloor()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit[] rayCastHits = Physics.RaycastAll(ray, 20f);

        foreach (RaycastHit hit in rayCastHits)
        {
            if (hit.collider.tag == "Floor")
            {
                transform.position = new Vector3(transform.position.x, hit.point.y + transform.lossyScale.y / 2, transform.position.z);
            }
        }
    }
}