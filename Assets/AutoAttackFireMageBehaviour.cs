using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackFireMageBehaviour : MonoBehaviour {

private void OnCollisionEnter(Collision collision)
    {
		EntityLivingBase entityHit = collision.gameObject.GetComponent<EntityLivingBase>();
        if (entityHit != null && entityHit.gameObject.tag != "Player")
        {
			transform.parent.GetComponent<AutoAttackFireMage>().OnAttackHit();
        }
		
		Destroy(this.gameObject);
    }
}
