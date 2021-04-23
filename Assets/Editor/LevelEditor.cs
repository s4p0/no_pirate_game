using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PerlinNoise))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var _class = target as PerlinNoise;
        if (GUILayout.Button("Generate terrain"))
            _class.GenerateTexture();

        if (GUILayout.Button("Clear TileMaps"))
            _class.Clear();
    }
    
}
