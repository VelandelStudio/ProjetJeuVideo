using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltageStatus : MonoBehaviour {


    int numberOfStacks;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddStacks(int number)
    {
        numberOfStacks += number;
    }

    void RemoteStacks()
    {
        numberOfStacks = 0;
    }
}
