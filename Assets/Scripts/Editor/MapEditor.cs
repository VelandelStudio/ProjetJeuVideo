using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (MapGenerator))]
public class NewBehaviourScript : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator map = target as MapGenerator;

        map.GenerationMap();
    }
}