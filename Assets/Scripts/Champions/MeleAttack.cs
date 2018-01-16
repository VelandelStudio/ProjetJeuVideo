using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MeleAttack class
/// This Abstract class contains all methods to do an Attack with a physical weapon (sword, spear etc...)
/// </summary>
//[RequireComponent(typeof(Collider))]
public class MeleAttack : MonoBehaviour {

    public bool colArme { get; set; }
    protected Collider _arme;

    /// <summary>
    /// Start Method
    /// Each physical weapon need to have a collider for checking collision
    /// The Start have to store in a variable this Collider and disable it.
    /// </summary>
    protected virtual void Start()
    {
        _arme = GetComponent<Collider>();
        colArme = _arme.enabled;
    }

    /// <summary>
    /// OnTriggerEnter Unity Method
    /// Will be called in the child class to Check if the Weapon touch an enemy
    /// </summary>
    /// <param name="other">is an Enemy (EntityLivingBase)</param>
    protected void OnTriggerEnter(Collider other)
    {

    }

    /// <summary>
    /// SwapEnableArmeCol method
    /// the aim of this method is to enable the collider of the weapon if is disable
    /// It will disable the weapon if it is enable.
    /// Generaly used when the player AutoAttack
    /// </summary>
    public void SwapEnableArmeCol()
    {
        _arme.enabled = !_arme.enabled;
        colArme = !colArme;
    }

    /*
    /// <summary>
    /// Getter of the Collider's weapon
    /// </summary>
    /// <returns>if the weapon Collider is enable or not</returns>
    public bool GetValueColArme()
    {
        return _arme.enabled;
    }
    */
}
