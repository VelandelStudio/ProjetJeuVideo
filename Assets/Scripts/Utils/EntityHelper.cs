using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}