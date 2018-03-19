using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/** EntityHelper, public static class
 * This UtilClass is used to contains all static method that can be applied on entities.
 **/
public static class EntityHelper
{

    /** ClearAllStatus public static void
	 * @param GameObject
	 * This method is used to clear all status on a gameObject. We parse the children and for each Status found, we destroy it.
	 **/
    public static void ClearAllStatus(GameObject obj)
    {
        IStatus[] status = obj.GetComponentsInChildren<IStatus>();
        for (int i = 0; i < status.Length; i++)
        {
            status[i].DestroyStatus();
        }
    }

    public static GameObject ApplyStatus(GameObject launcher, GameObject recev, GameObject statusObj)
    {
        StatusBase status = statusObj.GetComponent<StatusBase>();
        if (!CheckForStatusPresence(recev, status))
        {
            statusObj = GameObject.Instantiate(statusObj, recev.transform);
            status.AttributeCharacteristics(launcher.GetComponent<Characteristics>());
        }
        return statusObj;
    }

    public static bool CheckForStatusPresence(GameObject target, StatusBase other)
    {
        StatusBase[] statusOnTarget = target.transform.parent.GetComponentsInChildren<StatusBase>(true);
        for (int i = 0; i < statusOnTarget.Length; i++)
        {
            if (statusOnTarget[i].Name == other.Name)
            {
                if (target.transform.GetComponentInParent<IProjectile>() != null)
                {
                    return true;
                }

                if (target.transform.parent.gameObject.tag == "Player" && statusOnTarget[i].CurrentTimer > other.Duration - 1f)
                {
                    return true;
                }
                else
                {
                    statusOnTarget[i].RefreshStatus();
                    return true;
                }
            }
        }
        return false;
    }
}