using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBarBehaviour : MonoBehaviour {


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
