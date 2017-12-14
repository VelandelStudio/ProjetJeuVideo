using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** IDisplayable, public interface
 * Interface that regroups elements that can have a description on the screen.
 **/
public interface IDisplayable
{
    string Name { get; }
    string Element { get; }
    float CoolDownValue { get; }
    int[] Damages { get; }
    string[] DamagesType { get; }
    string[] OtherValues { get; }
    GameObject[] Status { get; }
    string[] Description { get; }
}