using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class SkillsetEditorManager : MonoBehaviour
{
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

    public CanvasGroup activeAbilities;
    public CanvasGroup availableAbilities;

    private static SkillsetEditorManager _skillsetEditor;
    private int _abilityIndex = -1;
    private bool _abilityChoosed = false;

    // When the user clicks on an available ability
    public void OnAbilityUIClick(Ability ability)
    {
        CombatManager.instance.GetActiveFighter().ChangeAbility(_abilityIndex, ability);
        _abilityChoosed = true;
    }
    
    // When the user clicks on an active ability
    public void SetClickedAbility(int abilityIndex)
    {
        _abilityIndex = abilityIndex;
        StartCoroutine(WaitForPlayerChoice());
    }

    private IEnumerator WaitForPlayerChoice()
    {
        while (!_abilityChoosed)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Unselect available abilities canvas group and set focus on actual abilities canvas group
                availableAbilities.interactable = false;
                activeAbilities.interactable = true;
                break;
            }

            yield return null;
        }
    }

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

    private void UpdateUI(dynamic obj)
    {
        Fighter activeFighter = obj.fighter;

        if (activeFighter.leftArm != null)
        {
            // LeftArmAbilities
            LeftArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftArm.abilities[0].abilityName;
            LeftArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftArm.abilities[1].abilityName;
        }
        else
        {
            LeftArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            LeftArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }

        if (activeFighter.rightArm != null)
        {
            // RightArmAbilities
            RightArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightArm.abilities[0].abilityName;
            RightArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightArm.abilities[1].abilityName;
        }
        else
        {
            RightArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            RightArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }

        if (activeFighter.leftShoulder != null)
        {
            // LeftShoulderAbilities
            LeftShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftShoulder.abilities[0].abilityName;
            LeftShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftShoulder.abilities[1].abilityName;
        }
        else
        {
            LeftShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            LeftShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }

        if (activeFighter.rightShoulder != null)
        {
            // RightShoulderAbilities
            RightShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightShoulder.abilities[0].abilityName;
            RightShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightShoulder.abilities[1].abilityName;
        }
        else
        {
            RightShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            RightShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
        }
    }
}
