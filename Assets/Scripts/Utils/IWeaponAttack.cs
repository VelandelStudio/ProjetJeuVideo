using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describe the family of attack done directly with a Mele Weapon (Swords ...)
/// It contains the OnTriggerEnter Method whitch calculate collision between weapon and entities
/// SwapEnableArmeCol changes the faculty of a weapon to apply effects
/// GetValueColArme returns faculty to a weapon to apply effects
/// </summary>
public interface IWeaponAttack {

    void OnTriggerEnter(Collider other);
    void SwapEnableArmeCol();
    bool GetValueColArme();
}
