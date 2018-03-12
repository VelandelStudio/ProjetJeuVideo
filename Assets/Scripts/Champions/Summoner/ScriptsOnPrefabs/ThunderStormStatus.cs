using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStormStatus : StatusBase 
{

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void OnStatusApplied()
    {
        Debug.Log("ThunderStormStatus successfully applied");
    }

    public override void StatusTickBehaviour()
    {

    }
}
