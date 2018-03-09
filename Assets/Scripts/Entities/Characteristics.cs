using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Characteristics, public class
 * This class contains all characteristics of an entitylivingBase or Champion.
 * This class should be only full of fields and properties that will be accessed by other scripts to calculate elements
 * such as damages, CD, Haste etc...
 **/
public class Characteristics : MonoBehaviour
{

    private float defense;
    public float Defense { get { return defense; } set { defense = value; } }

    private float movementSpeedFactor;
    public float MovementSpeedFactor { get { return movementSpeedFactor; } set { movementSpeedFactor = value; } }

    private float damageFactor;
    public float DamageFactor { get { return damageFactor; } set { damageFactor = value; } }

    private void Start()
    {
        Defense = 0f;
        MovementSpeedFactor = 1.0f;
        DamageFactor = 1.0f;
    }
}