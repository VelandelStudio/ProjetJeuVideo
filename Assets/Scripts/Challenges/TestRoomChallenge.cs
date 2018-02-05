using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoomChallenge : ChallengeBase
{
    private RoomBehaviour _roomBehavior;

    protected override void Start()
    {
        base.Start();

        _roomBehavior = GetComponent<RoomBehaviour>();
        Debug.Log(challengeData.Name);
    }

    public override bool ConditionToSucced()
    {
        if (_roomBehavior.IsClean)
        {
            return true;
        }

        return false;
    }
}
