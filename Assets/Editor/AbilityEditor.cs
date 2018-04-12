using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ability))]
public class AbilityEditor : Editor
{
    Ability ability;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Change things"))
        {
            Debug.Log("Debug");
        }
    }
}
