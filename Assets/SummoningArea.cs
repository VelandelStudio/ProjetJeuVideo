using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningArea : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Champion champion = other.GetComponent<Champion>();
            if(champion)
            {
                champion.DestroyChampion();
            }
        }
    }
}
