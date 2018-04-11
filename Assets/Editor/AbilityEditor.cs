using UnityEngine;
using System.Collections;
using UnityEditor;


public class AbilityEditor
{
    [MenuItem("Assets/Scriptable/Ability")]
    public static void CreateMyAsset()
    {
        Ability asset = ScriptableObject.CreateInstance<Ability>();

        AssetDatabase.CreateAsset(asset, "Assets/move_1.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
