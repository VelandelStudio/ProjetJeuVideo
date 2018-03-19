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
    private float _power;
    public float Power { get { return _power; } set { _power = value; } }

    private float _defense;
    public float Defense { get { return _defense; } set { _defense = value; } }

    private float _movementSpeedFactor;
    public float MovementSpeedFactor { get { return _movementSpeedFactor; } set { _movementSpeedFactor = value; } }

    private float _damageFactor;
    public float DamageFactor { get { return _damageFactor; } set { _damageFactor = value; } }

    private float _defenseFactor;
    public float DefenseFactor { get { return _defenseFactor; } set { _defenseFactor = value; } }

    private float _critChance;
    public float CritChance { get { return _critChance; } set { _critChance = value; } }

    private float _coolDownFactor;
    public float CoolDownFactor { get { return _coolDownFactor; } set { _coolDownFactor = value; } }

    private void Start()
    {
        Power = 200f;
        Defense = 5f;
        MovementSpeedFactor = 1.0f;
        DamageFactor = 1.0f;
        DefenseFactor = 2.0f;
        CritChance = 50.0f;
        CoolDownFactor = 1.0f;
    }
}