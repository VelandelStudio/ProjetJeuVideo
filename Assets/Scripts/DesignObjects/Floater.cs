using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Floater, public class
 * This class is used for Designing elements in the game.
 * When a GameObject is using this class, we allows him to "float" up to down and down to up. 
 * It is aloso able to set a rotation to the gameObject. We use it to make elements such as rocks or loot moving in the world space
 **/
public class Floater : MonoBehaviour
{

    [SerializeField] private float degreesPerSecond = 15.0f;
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    /** Start private void method
	 * We get the original position of the object.
	 **/
    private void Start()
    {
        posOffset = transform.position;
    }

    /** Update, private void method 
	 * We rotate and move the gameObject every frame.
	 **/
    private void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f));

        tempPos = posOffset;
        tempPos.y += Mathf.Cos(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}