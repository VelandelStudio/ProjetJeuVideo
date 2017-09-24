using UnityEngine;

public class ChargeSpell : Spell {
	
	private Vector3 directionOfCharge;
	private CameraController cameraPlayer;
	private float tick = 0;
	private float duration = 3.0f;
	private Collider obstacle;
	protected override void Start(){
		SpellCD = 10.0f;
		cameraPlayer = gameObject.GetComponentInChildren<Camera>().GetComponent<CameraController>();
		base.Start();	
	}
	
	public override void LaunchSpell() {
		base.LaunchSpell();
		if(!IsSpellLauncheable())
			return;
			
		directionOfCharge = transform.forward;
		cameraPlayer.CameraControlled = true;
		cameraPlayer.ControlCamera(cameraPlayer.GetCameraX(),0.5f);
	}
	
	protected override void Update() {
		base.Update();
		if(SpellInUse) {
			tick += Time.deltaTime;
			transform.position = transform.position + (directionOfCharge * 20f * Time.deltaTime * (tick*50/100));
		}
		
		if(tick >= duration)
			EndSpell();
	}
	
	private void OnCollisionEnter(Collision collision) {
		GameObject obstacle = collision.gameObject;
		EntityLivingBase livingObstacle = obstacle.GetComponent<EntityLivingBase>();
		if(livingObstacle != null && obstacle.tag != "Player")
			livingObstacle.DamageFor((int)(10*tick));
		
		EndSpell();
	}
	
	private void EndSpell() {
		cameraPlayer.CameraControlled = false;
		OnSpellLaunched();
		tick = 0;
	}
	

}