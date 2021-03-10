using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
[CustomEditor(typeof(Spawner))]


public class EditorSpawn : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Spawner _spawner = (Spawner)target;
        if (GUILayout.Button("Spawn"))
        {
            _spawner.Spawn();
        }
    }
}
