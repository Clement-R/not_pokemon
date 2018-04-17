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

        ability.abilityName = EditorGUILayout.TextField("Ability name", ability.abilityName);
        ability.type = (Ability.AbilityType) EditorGUILayout.EnumPopup("Type", ability.type);
        ability.target = (Ability.AbilityTarget)EditorGUILayout.EnumPopup("Target", ability.target);
        ability.effect = (GameObject) EditorGUILayout.ObjectField("Effect", ability.effect, typeof(GameObject), false);

        if (ability.type == Ability.AbilityType.ATTACK)
        {
            ability.damage = EditorGUILayout.IntField("Damage", ability.damage);
            ability.attackType = (Ability.AbilityAttackType)EditorGUILayout.EnumPopup("Attack type", ability.attackType);

            //if (GUILayout.Button("Change things for ATTACK"))
            //{
            //    Debug.Log("Debug");
            //}
        }
        else
        {
            ability.heal = EditorGUILayout.IntField("Heal", ability.heal);

            //if (GUILayout.Button("Change things for SUPPORT"))
            //{
            //    Debug.Log("Debug");
            //}
        }

        // EditorGUILayout.EnumPopup(ability.type);
    }
}
