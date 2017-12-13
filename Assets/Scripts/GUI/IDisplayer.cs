using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDisplayer
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