using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltageStatus : StatusBase, IBuff
{
    public void AddStacks(int number)
    {
        NumberOfStacks += number;
    }

    public void RemoteStacks()
    {
        NumberOfStacks = 0;
    }

    public int GetNumberOfStacks()
    {
        return NumberOfStacks;
    }

    public override void OnStatusApplied()
    {
        Debug.Log("VoltageStatus successfully applied");
    }

    public override void StatusTickBehaviour() {}
}
