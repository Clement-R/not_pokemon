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
    [MenuItem("Assets/Scriptable/Buff/HoT")]
    public static void CreateHoTBuff()
    {
        ScriptableCreatorUtility.CreateAsset<HealOverTime>();
    }

    [MenuItem("Assets/Scriptable/Debuff/DoT")]
    public static void CreateDoTDebuff()
    {
        ScriptableCreatorUtility.CreateAsset<BurnOverTime>();
    }

    [MenuItem("Assets/Scriptable/Modificator")]
    public static void CreateModificator()
    {
        ScriptableCreatorUtility.CreateAsset<ModificatorStatus>();
    }

    [MenuItem("Assets/Scriptable/Skillset")]
    public static void CreateSkillset()
    {
        ScriptableCreatorUtility.CreateAsset<Skillset>();
    }

    [MenuItem("Assets/Scriptable/MapEvent/CombatMapEvent")]
    public static void CreateCombatMapEvent()
    {
        ScriptableCreatorUtility.CreateAsset<CombatMapEvent>();
    }

    /*
    [MenuItem("Assets/Scriptable/Debuff")]
    public static void CreateDebuff()
    {
        ScriptableCreatorUtility.CreateAsset<Debuff>();
    }
    */
}
