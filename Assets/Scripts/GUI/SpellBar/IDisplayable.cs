using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** IDisplayable, public interface
 * Interface that regroups elements that can have a description on the screen.
 **/
public interface IDisplayable
{
    string Name { get; }
    
    string[] OtherValues { get; }
    string[] Description { get; }
    string GetDescriptionGUI();
    bool IsLoaded { get; }
}