using UnityEngine;
using System.Collections;
using UnityEditor;

public class ScriptableCreator
{
    [MenuItem("Assets/Scriptable/Ability")]
    public static void CreateAbility()
    {
        ScriptableCreatorUtility.CreateAsset<Ability>();
    }

    [MenuItem("Assets/Scriptable/Frame")]
    public static void CreateFrame()
    {
        ScriptableCreatorUtility.CreateAsset<Frame>();
    }

    [MenuItem("Assets/Scriptable/Pilot")]
    public static void CreatePilot()
    {
        ScriptableCreatorUtility.CreateAsset<Pilot>();
    }

    [MenuItem("Assets/Scriptable/Part")]
    public static void CreatePart()
    {
        ScriptableCreatorUtility.CreateAsset<Part>();
    }


    // Statuses

    /*
    [MenuItem("Assets/Scriptable/Buff")]
    public static void CreateBuff()
    {
        ScriptableCreatorUtility.CreateAsset<Buff>();
    }

    [MenuItem("Assets/Scriptable/Debuff")]
    public static void CreateDebuff()
    {
        ScriptableCreatorUtility.CreateAsset<Debuff>();
    }
    */
}
