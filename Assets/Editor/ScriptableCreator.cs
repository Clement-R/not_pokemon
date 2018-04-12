using UnityEngine;
using System.Collections;
using UnityEditor;

public class ScriptableCreator
{
    [MenuItem("Assets/Scriptable/Ability")]
    public static void CreateAbility()
    {
        Ability asset = ScriptableObject.CreateInstance<Ability>();

        AssetDatabase.CreateAsset(asset, "Assets/move_1.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }

    [MenuItem("Assets/Scriptable/Fighter")]
    public static void CreateFighter()
    {
        FighterCharacter asset = ScriptableObject.CreateInstance<FighterCharacter>();

        AssetDatabase.CreateAsset(asset, "Assets/fighter_1.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
