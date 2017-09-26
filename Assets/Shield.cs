using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	private float ShieldValue;
	
	public float GetShieldValue() {
		return ShieldValue;
	}
	
	public void AddShieldValue(float ShieldValue)
	{
		this.ShieldValue += ShieldValue;
	}
	
	public void ReduceShieldValue(float ShieldValue)
	{
		this.ShieldValue -= ShieldValue;
		if(this.ShieldValue <= 0)
			RemoveShield();
	}
	
	public void RemoveShield() {
		Destroy(this);
	}
}
