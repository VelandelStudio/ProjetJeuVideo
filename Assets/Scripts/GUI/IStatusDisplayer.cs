using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/** IStatusDisplayble, public interface
 * @Extends : IDisplayer
 * Interface that regroups elements that can have a description on the screen, with specific properties foir Status
 **/
public interface IStatusDisplayable : IDisplayable
{
    float Duration { get; }
    bool IsTickable { get; }
    float[] TicksIntervals { get; }
    float[] TickStarts { get; }
}