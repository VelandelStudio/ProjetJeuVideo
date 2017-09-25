using System.Collections;
using UnityEditor;
using UnityEngine;

/** MapEditorScript Class
 *  @ Inherits Editor
 *  This is a script that check the presence of a MapGenerator Script inside the inspector
 *  and do directly all the modifications (good if you are testing and don't want to play the scene each time)
 **/
[CustomEditor (typeof (MapGenerator))]
public class MapEditor : Editor {

    /** OnInspectorGUI callback Méthode
     *  Check a MapEditor in the Inspector
     *  Use the GenerationMap Méthode if found
     **/
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map = target as MapGenerator;

        // map.GenerationMap();
    }
}