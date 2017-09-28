using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveFireMage : MonoBehaviour
{

    private ConflagrationSpell conflagrationSpell;
    private float NumberOfIgnites;
    public float CritChanceToAdd;

    private void Start()
    {
        conflagrationSpell = GetComponent<ConflagrationSpell>();
        NumberOfIgnites = conflagrationSpell.targets.Count;
    }

    private void Update()
    {
        NumberOfIgnites = conflagrationSpell.targets.Count;
        CritChanceToAdd = Mathf.Clamp(5 * NumberOfIgnites, 0, 25);
    }
}
