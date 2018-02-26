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
    [SerializeField] private float degreesPerSecond;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequency;

    [SerializeField] private bool floatOnStart = true;
    [SerializeField] private float startingTime;
    [SerializeField] private float verticalAdd;
    [SerializeField] private Vector3 targetRotation;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    private Vector3 startFloatingPos;

    /** Start private void method
	 * We get the original position of the object.
	 **/
    private void Start()
    {
        posOffset = transform.localPosition;
        startFloatingPos = posOffset + new Vector3(0f, verticalAdd, 0f);
    }

    /** Update, private void method 
	 * We rotate and move the gameObject every frame.
	 **/
    private void FixedUpdate()
    {
        if (floatOnStart && startingTime <= 0)
        {
            transform.Rotate(0f, Time.deltaTime * degreesPerSecond, 0f);
            tempPos = posOffset;
            tempPos.y += Mathf.Cos(Time.fixedTime * Mathf.PI * frequency) * amplitude;

            transform.localPosition = tempPos;
        }
        else
        {
            GoingUp();
        }
    }

    private void GoingUp()
    {
        if (!floatOnStart && startingTime > 0)
        {
            startingTime -= Time.deltaTime;
        }
        else
        {
            transform.Rotate(new Vector3(Time.deltaTime * targetRotation.x, 0f, Time.deltaTime * targetRotation.z));

            if (transform.localPosition.y < startFloatingPos.y)
            {
                posOffset.y += 0.5f * Time.fixedDeltaTime;
                transform.localPosition = posOffset;
            }
            else
            {
                tempPos = posOffset;
                tempPos.y += Mathf.Cos(Time.fixedTime * Mathf.PI * frequency) * amplitude;

                if (tempPos.y > transform.localPosition.y - 0.0005 && tempPos.y < transform.localPosition.y + 0.0005)
                {
                    floatOnStart = true;
                }
            }
        }
    }


}