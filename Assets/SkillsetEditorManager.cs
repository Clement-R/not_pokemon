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
        EventManager.StartListening(EventList.FIGHTER_STUFF_UPDATE.ToString(), UpdateUI);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventList.DISPLAY_SKILLSET_EDITOR.ToString(), UpdateUI);
        EventManager.StopListening(EventList.FIGHTER_STUFF_UPDATE.ToString(), UpdateUI);
    }

    void UpdateUI(dynamic obj)
    {
        Fighter activeFighter = obj.fighter;

        if(activeFighter.leftArm != null)
        {
            // LeftArmAbilities
            LeftArmAbilities[0].GetComponent<TMPro.TMP_Text>().text = activeFighter.leftArm.abilities[0].abilityName;
            LeftArmAbilities[1].GetComponent<TMPro.TMP_Text>().text = activeFighter.leftArm.abilities[1].abilityName;
        }

        if (activeFighter.rightArm != null)
        {
            // RightArmAbilities
            RightArmAbilities[0].GetComponent<TMPro.TMP_Text>().text = activeFighter.rightArm.abilities[0].abilityName;
            RightArmAbilities[1].GetComponent<TMPro.TMP_Text>().text = activeFighter.rightArm.abilities[1].abilityName;
        }

        if (activeFighter.leftShoulder != null)
        {
            // LeftShoulderAbilities
            LeftShoulderAbilities[0].GetComponent<TMPro.TMP_Text>().text = activeFighter.leftShoulder.abilities[0].abilityName;
            LeftShoulderAbilities[1].GetComponent<TMPro.TMP_Text>().text = activeFighter.leftShoulder.abilities[1].abilityName;
        }

        if (activeFighter.rightShoulder != null)
        {
            // RightShoulderAbilities
            RightShoulderAbilities[0].GetComponent<TMPro.TMP_Text>().text = activeFighter.rightShoulder.abilities[0].abilityName;
            RightShoulderAbilities[1].GetComponent<TMPro.TMP_Text>().text = activeFighter.rightShoulder.abilities[1].abilityName;
        }
    }
}
