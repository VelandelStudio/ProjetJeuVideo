using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class PNJBehaviour : EntityLivingBase, IInterractableEntity {

    private void Start()
    {
        InitializeLivingEntity(100, 100);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;

        Transform target = other.transform;
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime *10);
    }

    public void ActivateInterractable()
    {
        Debug.Log("Hello Sir ! I'm the first PNJ in this game !");
    }

    public void DisplayTextOfInterractable()
    {
        Debug.Log("Press " + InputsProperties.activate.ToString() + " to activate.");
    }
}
