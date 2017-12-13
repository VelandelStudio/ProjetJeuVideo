using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStatusDisplayer : IDisplayer
{
    float Duration { get; }
    bool IsTickable { get; }
    float[] TicksIntervals { get; }
    float[] TickStarts { get; }
}
