using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveWindiator : PassiveBase {

    /// <summary>
    /// ProcPassive Method
    /// Just here to call the ApplyStatus on WindiatorPassive
    /// </summary>
    /// <param name="player">A player in the game</param>
    public void ProcPassive(GameObject player)
    {
        ApplyStatus(Status[0], player.transform);
        Debug.Log(Status[0].name + " applied");
    }
}
