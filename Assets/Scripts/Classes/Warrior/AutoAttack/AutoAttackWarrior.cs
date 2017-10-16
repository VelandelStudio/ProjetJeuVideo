using UnityEngine;

public class AutoAttackWarrior : AutoAttackBase
{
	protected override void Start() { 
		GCD = 1.5f;
		base.Start();
	}
	
	public override void AutoAttack() {
		if(AutoAttackIsReady()) {
			Debug.Log("AutoAttackWarrior Launched");
			base.AutoAttack();
		}
	}
}