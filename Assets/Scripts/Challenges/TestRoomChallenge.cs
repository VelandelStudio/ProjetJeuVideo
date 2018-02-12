using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomChallenge : ChallengeBase
{
    public override bool ConditionToSucced()
    {
        return (roomBehavior.IsClean);
    }

    public override void GiveReward()
    {
        GetComponentInParent<DungeonManager>().AddChallengeBonus(5);
    }
}
