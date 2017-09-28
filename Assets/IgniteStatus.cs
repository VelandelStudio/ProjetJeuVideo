using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteStatus : MonoBehaviour
{
    private float MaxDuration = 500;
    private float TickInterval = 1;
    private float Tick = 0;

    private EntityLivingBase entity;
    private GameObject particles;
    private void Start()
    {
        GameObject obj = (GameObject)Resources.Load("SpellPrefabs/IgniteStatus", typeof(GameObject));
        particles = Instantiate(obj, transform.position, transform.rotation, transform);

        entity = GetComponent<EntityLivingBase>();
        Invoke("EndStatus", (int)MaxDuration);
    }

    private void Update()
    {
        Tick += Time.deltaTime;
        if (Tick >= TickInterval)
        {
            Tick--;
            entity.DamageFor(5);
        }
    }

    public void ResetStatus()
    {
        CancelInvoke("EndStatus");
        Tick = 0;
        Invoke("EndStatus", (int)MaxDuration);
    }


    public void EndStatus()
    {
        Destroy(particles);
        Destroy(this);
    }

}