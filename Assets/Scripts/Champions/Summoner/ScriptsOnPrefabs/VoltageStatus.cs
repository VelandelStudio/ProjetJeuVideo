using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltageStatus : StatusBase, IBuff
{

    public int numberOfStacks;
    
    // Use this for initialization
	public void Start () {
        numberOfStacks = 0;
	}
	
	// Update is called once per frame
	public void Update () {
 
    }

    public void AddStacks(int number)
    {
        numberOfStacks += number;
    }

    public void RemoteStacks()
    {
        numberOfStacks = 0;
    }

    public int GetNumberOfStacks()
    {
        return numberOfStacks;
    }

    public override void OnStatusApplied()
    {
        Debug.Log("VoltageStatus successfully applied");
    }

    public override void StatusTickBehaviour()
    {
        
    }
}
