using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAtlas : MonoBehaviour {

    [SerializeField] private GameObject _objToRotateAround;
    public Vector3 RotateValues;
    public float MoveSpeed;

    private void FixedUpdate ()
    {
        transform.RotateAround(_objToRotateAround.transform.position, RotateValues, MoveSpeed * Time.deltaTime);
        transform.LookAt(_objToRotateAround.transform);
	}
}
