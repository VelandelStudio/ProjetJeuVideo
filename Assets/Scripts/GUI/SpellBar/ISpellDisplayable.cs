using UnityEngine;

/** ISpellDisplayable, public interface
 * @implements : IDisplayable
 * Interface that regroups elements that can have a description on the screen.
 * These elements are spells like, meaning they have an element, type, CD, damages and status.
 **/
public interface ISpellDisplayable : IDisplayable {
    string Element { get; }
    string Type { get; }
    float CoolDownValue { get; }
    int[] Damages { get; }
    string[] DamagesType { get; }
    GameObject[] Status { get; }
}
