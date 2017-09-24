using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteStatus : MonoBehaviour {
    private float MaxDuration = 500;
    private float TickInterval = 1;
    private float Tick = 0;
    private EntityLivingBase entity;
    public GameObject obj;
    private void Start () {
        obj = (GameObject)Resources.Load("FireComplex", typeof(GameObject));

        entity = GetComponent<EntityLivingBase>();
        Destroy(this, MaxDuration);
    }
	
	private void Update () {
        Tick += Time.deltaTime;
        if (Tick >= TickInterval)
        {
            Tick--;
            entity.DamageFor(5);
        }
	}
}
