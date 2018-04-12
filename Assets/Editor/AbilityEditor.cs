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

        ability = (Ability)target;

        if(ability.type == Ability.AbilityType.ATTACK)
        {
            if (GUILayout.Button("Change things for ATTACK"))
            {
                Debug.Log("Debug");
            }
        }
        else
        {
            if (GUILayout.Button("Change things for SUPPORT"))
            {
                Debug.Log("Debug");
            }
        }

        // EditorGUILayout.EnumPopup(ability.type);
    }
}
