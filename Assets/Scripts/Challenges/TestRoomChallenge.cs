using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomChallenge : ChallengeBase
{
    public override bool ConditionToSucced()
    {
        GetComponentInParent<DungeonManager>().AddChallengeBonus(5);
        return (roomBehavior.IsClean);
    }

}
