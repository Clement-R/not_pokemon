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

    public GameObject[] availableAbilities;

    public GameObject[] leftArmAbilities;
    public GameObject[] rightArmAbilities;
    public GameObject[] leftShoulderAbilities;
    public GameObject[] rightShoulderAbilities;

    public CanvasGroup activeAbilitiesCanvas;
    public CanvasGroup availableAbilitiesCanvas;

    private static SkillsetEditorManager _skillsetEditor;
    private int _abilityIndex = -1;
    private bool _abilityChoosed = false;

    // When the user clicks on an available ability
    public void OnAbilityUIClick(PartAbilityUI partUI)
    {
        CombatManager.instance.GetActiveFighter().ChangeAbility(_abilityIndex, partUI.ability);
        _abilityChoosed = true;

        UpdateUI(new { fighter = CombatManager.instance.GetActiveFighter() });

        // Unselect available abilities canvas group and set focus on actual abilities canvas group
        availableAbilitiesCanvas.interactable = false;
        activeAbilitiesCanvas.interactable = true;
    }
    
    // When the user clicks on an active ability
    public void SetClickedAbility(int abilityIndex)
    {
        _abilityIndex = abilityIndex;
        StartCoroutine(WaitForPlayerChoice());
    }

    public void OnHide()
    {
        EventManager.TriggerEvent(EventList.SHOW_COMBAT_UI.ToString(), new { });
    }

    private IEnumerator WaitForPlayerChoice()
    {
        while (!_abilityChoosed)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Unselect available abilities canvas group and set focus on actual abilities canvas group
                availableAbilitiesCanvas.interactable = false;
                activeAbilitiesCanvas.interactable = true;
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
            leftArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftArm.abilities[0].abilityName;
            leftArmAbilities[0].GetComponent<PartAbilityUI>().ability = activeFighter.leftArm.abilities[0];
            leftArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftArm.abilities[1].abilityName;
            leftArmAbilities[1].GetComponent<PartAbilityUI>().ability = activeFighter.leftArm.abilities[1];
        }
        else
        {
            leftArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            leftArmAbilities[0].GetComponent<PartAbilityUI>().ability = null;
            leftArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            leftArmAbilities[1].GetComponent<PartAbilityUI>().ability = null;
        }

        if (activeFighter.rightArm != null)
        {
            // RightArmAbilities
            rightArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightArm.abilities[0].abilityName;
            rightArmAbilities[0].GetComponent<PartAbilityUI>().ability = activeFighter.rightArm.abilities[0];
            rightArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightArm.abilities[1].abilityName;
            rightArmAbilities[1].GetComponent<PartAbilityUI>().ability = activeFighter.rightArm.abilities[1];
        }
        else
        {
            rightArmAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            rightArmAbilities[0].GetComponent<PartAbilityUI>().ability = null;
            rightArmAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            rightArmAbilities[1].GetComponent<PartAbilityUI>().ability = null;
        }

        if (activeFighter.leftShoulder != null)
        {
            // LeftShoulderAbilities
            leftShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftShoulder.abilities[0].abilityName;
            leftShoulderAbilities[0].GetComponent<PartAbilityUI>().ability = activeFighter.leftShoulder.abilities[0];
            leftShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.leftShoulder.abilities[1].abilityName;
            leftShoulderAbilities[1].GetComponent<PartAbilityUI>().ability = activeFighter.leftShoulder.abilities[1];
        }
        else
        {
            leftShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            leftShoulderAbilities[0].GetComponent<PartAbilityUI>().ability = null;
            leftShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            leftShoulderAbilities[1].GetComponent<PartAbilityUI>().ability = null;
        }

        if (activeFighter.rightShoulder != null)
        {
            // RightShoulderAbilities
            rightShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightShoulder.abilities[0].abilityName;
            rightShoulderAbilities[0].GetComponent<PartAbilityUI>().ability = activeFighter.rightShoulder.abilities[0];
            rightShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.rightShoulder.abilities[1].abilityName;
            rightShoulderAbilities[1].GetComponent<PartAbilityUI>().ability = activeFighter.rightShoulder.abilities[1];
        }
        else
        {
            rightShoulderAbilities[0].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            rightShoulderAbilities[0].GetComponent<PartAbilityUI>().ability = null;
            rightShoulderAbilities[1].GetComponentInChildren<TMPro.TMP_Text>().text = "";
            rightShoulderAbilities[1].GetComponent<PartAbilityUI>().ability = null;
        }

        for (int i = 0; i < 4; i++)
        {
            availableAbilities[i].GetComponentInChildren<TMPro.TMP_Text>().text = activeFighter.skillset._abilities[i].abilityName;
        }
    }
}
