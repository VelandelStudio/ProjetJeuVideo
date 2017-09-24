using UnityEngine;

public abstract class AutoAttackBase : MonoBehaviour
{
	protected float GCD;
	protected float CurrentGCD;	
	
	protected virtual void Start() { 
		CurrentGCD = GCD;
	}
	
	protected virtual void Update() {
		if(!AutoAttackIsReady())
			CurrentGCD = Mathf.Clamp(CurrentGCD + Time.deltaTime, 0, GCD);	
	}
	
	protected bool AutoAttackIsReady()
	{
		return (CurrentGCD == GCD);
	}
	
	public virtual void AutoAttack() {
		CurrentGCD = 0;
	}
}