using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public abstract class LinearProjectile : MonoBehaviour, IProjectile {

    protected Transform launcher;       //
    protected float _timeOfFly = 2f;    //

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        launcher = transform.parent;
        GetComponent<Collider>().isTrigger = true;
        transform.parent = null;

        LaunchProjectile();
        Destroy(gameObject, _timeOfFly);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="col"></param>
    protected void OnTriggerEnter(Collider col)
    {
        EntityLivingBase eHit = col.gameObject.GetComponent<EntityLivingBase>();

        if (eHit != null && eHit.gameObject.tag != "player")
        {
            ApplyEffect(col);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="col"></param>
    public abstract void ApplyEffect(Collider col);

    /// <summary>
    /// 
    /// </summary>
    public void LaunchProjectile()
    {
        Camera cameraPlayer = launcher.GetComponentInChildren<Camera>();

        RaycastHit hit;

        bool hasFoundHitPoint = Physics.Raycast(cameraPlayer.GetComponent<GameObjectDetector>().OriginPoint,
            cameraPlayer.transform.forward,
            out hit,
            Mathf.Infinity,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Ignore);

        Vector3 target;
        if (hasFoundHitPoint)
        {
            target = hit.point;
        }
        else
        {
            target = transform.position + cameraPlayer.transform.forward * 10;
        }

        transform.LookAt(target);
        GetComponent<Rigidbody>().AddForce(transform.forward * 1000);

        ParticleSystem particles = GetComponent<ParticleSystem>();

        if (particles != null)
        {
            particles.Play();
        }

    }
}
