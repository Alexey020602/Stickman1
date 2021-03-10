using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.TerrainAPI;
[CustomEditor(typeof(PlayerManager))]


public class EditorPlayerManager : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerManager PlayerManager = (PlayerManager)target;
        if (GUILayout.Button("Search Body Parts"))
        {
            PlayerManager.FindBodyesParts();
        }
    }
}