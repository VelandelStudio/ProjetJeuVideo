using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** IStatus public interface
 * Interface that contains all methods to apply a buff/debuff to a target.
 **/
public interface IStatus
{ 
    void OnStatusApplied();
    void StatusTickBehaviour();
    void ResetStatus();
    void DestroyStatus();
}
