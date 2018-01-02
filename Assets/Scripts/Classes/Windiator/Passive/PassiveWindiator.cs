using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveWindiator : PassiveBase {

    public void ProcPassive(GameObject player)
    {
        ApplyStatus(Status[0], player.transform);
        Debug.Log(Status[0].name + " applied");
    }
}
