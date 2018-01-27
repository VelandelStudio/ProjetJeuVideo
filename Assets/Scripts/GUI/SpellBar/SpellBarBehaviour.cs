using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** SpellBarBehaviour, public class
 * This class should only be associated with the SpellBar GUI Element.
 * It is used to enable or disable the spell bar and all its components.
 **/
public class SpellBarBehaviour : MonoBehaviour {

    /** SetChildrenActives, public void
     * @param : pool
     * We parse all ChildrenElements of the spell bar and activate or deactivate them.
     **/
    public void SetChildrenActives(bool active)
    {
        Transform[] tr = GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < tr.Length; i++)
        {
            if(tr[i] != transform)
            {
                tr[i].gameObject.SetActive(active);
            }
        }
    }
}
