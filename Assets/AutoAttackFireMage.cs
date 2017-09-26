using UnityEngine;

public class AutoAttackFireMage : AutoAttackBase
{
	public GameObject throwable;
	public Camera cameraPlayer;
	public Transform launcherTransform;
	
	private Shield shield;
	private float MaxDurationShield = 5;
	private float MaxValueShield = 50;
	private float tick = 0;
	
	private RaycastHit hit;
	private bool hasFoundHitPoint;
	private Vector3 target;
	private GameObject throwableInstance;

	protected override void Start() { 
		GCD = 1.5f;
		cameraPlayer = this.GetComponentInChildren<Camera>();
		throwable = (GameObject)Resources.Load("AutoAttackFireMage", typeof(GameObject));
		Transform[] transformTab = this.gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform tr in transformTab) {
			if(tr.gameObject.name == "EthanLeftHand") {
				launcherTransform = tr;
				break;
			}
		}
		base.Start();
	}
	
	public override void AutoAttack() {
		if(AutoAttackIsReady()) {
			hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
			if(hasFoundHitPoint) 
				target = hit.point;
			else 
				target = launcherTransform.position + cameraPlayer.transform.forward;
			throwableInstance = Instantiate(throwable,launcherTransform.position + cameraPlayer.transform.forward*2, launcherTransform.rotation, this.transform);

			throwableInstance.transform.LookAt(target);
			throwableInstance.GetComponent<Rigidbody>().AddForce(throwableInstance.transform.forward * 1000);

			Debug.Log("AutoAttackFireMage Launched");
			base.AutoAttack();
		}
	}
	
	public void OnAttackHit() {
		shield = GetComponent<Shield>();
		if(shield == null) 
			shield = gameObject.AddComponent<Shield>();
	
		shield.AddShieldValue(Mathf.Clamp(5,0,MaxValueShield-shield.GetShieldValue()));
		tick = 0;	
	}
	
	protected override void Update() { 
		base.Update();
		if(shield != null)
			tick += Time.deltaTime;
		
		if(tick >= MaxDurationShield) {
			shield.RemoveShield();
			tick = 0;
		}
	}
}