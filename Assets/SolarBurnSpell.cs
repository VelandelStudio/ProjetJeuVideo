using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarBurnSpell : Spell
{
    private Camera cameraPlayer;
    private Transform launcherTransform;
    private RaycastHit hit;
    private Vector3 target;
    private bool hasFoundHitPoint;


    protected override void Start()
    {
        cameraPlayer = this.GetComponentInChildren<Camera>();
        launcherTransform = this.transform;
        SpellCD = 7f;
        base.Start();
    }

    public override void LaunchSpell()
    {
        if (!IsSpellLauncheable())
            return;

        hasFoundHitPoint = Physics.Raycast(PosHelper.GetOriginOfDetector(transform), cameraPlayer.transform.forward, out hit, Mathf.Infinity, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (hasFoundHitPoint)
        {
            base.LaunchSpell();
            target = hit.point;
        }
        else
            return;

        GameObject throwable = (GameObject)Resources.Load("SpellPRefabs/FireBall", typeof(GameObject));

        Vector2 pointInCircle = Random.insideUnitCircle.normalized * 8;
        Vector3 v = new Vector3(target.x + pointInCircle.x, target.y + 10, target.z + pointInCircle.y);
        GameObject throwableInstance = Instantiate(throwable, v, new Quaternion(), this.transform);

        throwableInstance.transform.LookAt(target);
        throwableInstance.GetComponent<Rigidbody>().AddForce(throwableInstance.transform.forward * 1000);
        ParticleSystem particles = throwableInstance.GetComponent<ParticleSystem>();
        particles.Play();

        base.OnSpellLaunched();
    }
}
