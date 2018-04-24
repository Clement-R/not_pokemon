using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class SkillsetEditorManager : MonoBehaviour {

    public static SkillsetEditorManager instance
    {
        get
        {
            if (!_skillsetEditor)
            {
                _skillsetEditor = FindObjectOfType(typeof(SkillsetEditorManager)) as SkillsetEditorManager;

                if (!_skillsetEditor)
                {
                    Debug.LogError("There needs to be one active SkillsetEditorManager script on a GameObject in your scene.");
                }
            }

            return _skillsetEditor;
        }
    }

    public GameObject[] LeftArmAbilities;
    public GameObject[] RightArmAbilities;
    public GameObject[] LeftShoulderAbilities;
    public GameObject[] RightShoulderAbilities;

    private static SkillsetEditorManager _skillsetEditor;

    private void OnEnable()
    {
        EventManager.StartListening(EventList.DISPLAY_SKILLSET_EDITOR.ToString(), UpdateUI);
    }

    void UpdateUI(dynamic obj)
    {
        Fighter activeFighter = obj.fighter;

        if(activeFighter.leftArm != null)
        {
            // LeftArmAbilities
            // activeFighter.leftArm.abilities[0]
            // activeFighter.leftArm.abilities[1]
        }

        if (activeFighter.rightArm != null)
        {
            // RightArmAbilities
            // activeFighter.rightArm.abilities[0]
            // activeFighter.rightArm.abilities[1]
        }

        if (activeFighter.leftShoulder != null)
        {
            // LeftShoulderAbilities
            // activeFighter.leftShoulder.abilities[0]
            // activeFighter.leftShoulder.abilities[1]
        }

        if (activeFighter.rightShoulder != null)
        {
            // RightShoulderAbilities
            // activeFighter.rightShoulder.abilities[0]
            // activeFighter.rightShoulder.abilities[1]
        }
    }
}
