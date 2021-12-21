using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class GeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator map = (MapGenerator)target;
        float x = EditorGUILayout.Slider("xSize", map.xSize, 20, 100);
        float z = EditorGUILayout.Slider("zSize", map.zSize, 20, 100);
        map.xSize = Mathf.FloorToInt(x );
        map.zSize = Mathf.FloorToInt( z);
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Simple Map"))
        {
            map.StartTest();
        }
    }
}
